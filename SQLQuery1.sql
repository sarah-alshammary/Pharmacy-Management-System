/* =========================================================
   Pharmacy DB - Clean Script (SQL Server)
   - Uses dbo.users as the pharmacists table (userRole='Pharmacist')
   - Creates core tables, constraints, indexes, procedures
   - Optional: medic sync (only if you still need medic table)
========================================================= */

----------------------------------------------------------
-- 0) Database
----------------------------------------------------------
IF DB_ID('pharmacy') IS NULL
    CREATE DATABASE pharmacy;
GO
USE pharmacy;
GO

----------------------------------------------------------
-- 1) Core tables
----------------------------------------------------------

/* Users (Admin + Pharmacist) */
IF OBJECT_ID('dbo.[users]','U') IS NULL
BEGIN
    CREATE TABLE dbo.[users](
        id       INT IDENTITY(1,1) PRIMARY KEY,
        userRole VARCHAR(50)  NOT NULL,
        [name]   VARCHAR(250) NOT NULL,
        dob      VARCHAR(250) NOT NULL,
        mobile   BIGINT       NOT NULL,
        email    VARCHAR(250) NOT NULL,
        username VARCHAR(250) NOT NULL UNIQUE,
        [pass]   VARCHAR(250) NOT NULL
    );
END
GO

/* Customers */
IF OBJECT_ID('dbo.Customers','U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers(
        CustomerID   INT IDENTITY(1,1) PRIMARY KEY,
        Username     VARCHAR(250) NOT NULL UNIQUE,
        Email        VARCHAR(250) NOT NULL,
        Mobile       BIGINT       NOT NULL,
        [Password]   VARCHAR(250) NOT NULL,
        PharmacistID INT NULL      -- assigned pharmacist (users.id)
    );
END
GO

/* Medicines */
IF OBJECT_ID('dbo.Medicines','U') IS NULL
BEGIN
    CREATE TABLE dbo.Medicines(
        MedicineID    INT IDENTITY(1,1) PRIMARY KEY,
        MedName       VARCHAR(250) NOT NULL UNIQUE,
        Price         DECIMAL(10,2) NOT NULL,
        [Description] NVARCHAR(500) NULL
    );
END
GO

/* CustomerMedicines (Prescriptions) */
IF OBJECT_ID('dbo.CustomerMedicines','U') IS NULL
BEGIN
    CREATE TABLE dbo.CustomerMedicines(
        CustomerMedicineID        INT IDENTITY(1,1) PRIMARY KEY,
        CustomerID                INT NOT NULL,
        MedicineID                INT NOT NULL,
        TimesPerDay               TINYINT NOT NULL,
        UnitsPerDose              DECIMAL(10,2) NOT NULL,
        StartDate                 DATE NOT NULL CONSTRAINT DF_CM_StartDate DEFAULT (CAST(GETDATE() AS DATE)),
        DurationDays              INT  NOT NULL CONSTRAINT DF_CM_DurationDays DEFAULT (7),
        PrescribedByPharmacistID  INT  NOT NULL,
        -- EndDate is computed from StartDate + DurationDays
        EndDate AS (DATEADD(DAY, DurationDays - 1, StartDate)) PERSISTED
    );
END
GO

----------------------------------------------------------
-- 2) Foreign keys (create only if missing)
----------------------------------------------------------

/* Customers -> Users (assigned pharmacist) */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Customers_Users')
BEGIN
    ALTER TABLE dbo.Customers WITH CHECK
    ADD CONSTRAINT FK_Customers_Users
    FOREIGN KEY (PharmacistID) REFERENCES dbo.[users](id);
END
GO

/* CustomerMedicines -> Customers, Medicines */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CM_Customers')
BEGIN
    ALTER TABLE dbo.CustomerMedicines WITH CHECK
    ADD CONSTRAINT FK_CM_Customers
    FOREIGN KEY (CustomerID) REFERENCES dbo.Customers(CustomerID);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CM_Medicines')
BEGIN
    ALTER TABLE dbo.CustomerMedicines WITH CHECK
    ADD CONSTRAINT FK_CM_Medicines
    FOREIGN KEY (MedicineID) REFERENCES dbo.Medicines(MedicineID);
END
GO

/* CustomerMedicines -> Users (prescribing pharmacist) */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CM_Users')
BEGIN
    ALTER TABLE dbo.CustomerMedicines WITH CHECK
    ADD CONSTRAINT FK_CM_Users
    FOREIGN KEY (PrescribedByPharmacistID) REFERENCES dbo.[users](id);
END
GO

----------------------------------------------------------
-- 3) Pharmacist inventory
----------------------------------------------------------

IF OBJECT_ID('dbo.PharmacistMedicines','U') IS NULL
BEGIN
    CREATE TABLE dbo.PharmacistMedicines(
        ID           INT IDENTITY(1,1) PRIMARY KEY,
        PharmacistID INT NOT NULL,
        MedicineID   INT NOT NULL,
        QtyAvailable DECIMAL(10,2) NOT NULL,
        CONSTRAINT FK_PM_Users     FOREIGN KEY (PharmacistID) REFERENCES dbo.[users](id),
        CONSTRAINT FK_PM_Medicines FOREIGN KEY (MedicineID)   REFERENCES dbo.Medicines(MedicineID),
        CONSTRAINT UX_PM UNIQUE(PharmacistID, MedicineID) -- no duplicates
    );
END
GO

----------------------------------------------------------
-- 4) Helpful indexes
----------------------------------------------------------

/* Index for pharmacist -> customers */
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name='IX_Customers_Pharmacist' AND object_id=OBJECT_ID('dbo.Customers')
)
    CREATE INDEX IX_Customers_Pharmacist ON dbo.Customers(PharmacistID);
GO

/* Index for pharmacist inventory */
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name='IX_PM_Pharmacist' AND object_id=OBJECT_ID('dbo.PharmacistMedicines')
)
    CREATE INDEX IX_PM_Pharmacist ON dbo.PharmacistMedicines(PharmacistID);
GO

/* Index for customer prescriptions */
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name='IX_CM_Customer' AND object_id=OBJECT_ID('dbo.CustomerMedicines')
)
    CREATE INDEX IX_CM_Customer ON dbo.CustomerMedicines(CustomerID);
GO

----------------------------------------------------------
-- 5) Stored procedures
----------------------------------------------------------

/* Get customers for a pharmacist */
IF OBJECT_ID('dbo.sp_GetPharmacistCustomers','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetPharmacistCustomers;
GO
CREATE PROCEDURE dbo.sp_GetPharmacistCustomers
    @PharmacistID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CustomerID, Username, Email, Mobile
    FROM dbo.Customers
    WHERE PharmacistID = @PharmacistID
    ORDER BY Username;
END
GO

/* Get pharmacist inventory (optional search) */
IF OBJECT_ID('dbo.sp_GetPharmacistInventory','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetPharmacistInventory;
GO
CREATE PROCEDURE dbo.sp_GetPharmacistInventory
    @PharmacistID INT,
    @q NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  pm.ID AS InventoryID,
            m.MedicineID,
            m.MedName,
            m.[Description],
            m.Price,
            pm.QtyAvailable
    FROM dbo.PharmacistMedicines pm
    JOIN dbo.Medicines m ON m.MedicineID = pm.MedicineID
    WHERE pm.PharmacistID = @PharmacistID
      AND (@q IS NULL OR m.MedName LIKE '%' + @q + '%')
    ORDER BY m.MedName;
END
GO

/* Get customer prescriptions */
IF OBJECT_ID('dbo.sp_GetCustomerPrescriptions','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetCustomerPrescriptions;
GO
CREATE PROCEDURE dbo.sp_GetCustomerPrescriptions
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  cm.CustomerMedicineID,
            m.MedName,
            cm.TimesPerDay,
            cm.UnitsPerDose,
            cm.StartDate,
            cm.DurationDays,
            cm.EndDate,
            u.username AS PrescribedBy
    FROM dbo.CustomerMedicines cm
    JOIN dbo.Medicines m ON m.MedicineID = cm.MedicineID
    JOIN dbo.[users] u ON u.id = cm.PrescribedByPharmacistID
    WHERE cm.CustomerID = @CustomerID
    ORDER BY cm.StartDate DESC, cm.CustomerMedicineID DESC;
END
GO

/* Prescribe a medicine + deduct inventory */
IF OBJECT_ID('dbo.sp_PrescribeMedicine','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_PrescribeMedicine;
GO
CREATE PROCEDURE dbo.sp_PrescribeMedicine
    @PharmacistID  INT,
    @CustomerID    INT,
    @MedicineID    INT,
    @TimesPerDay   TINYINT,
    @UnitsPerDose  DECIMAL(10,2),
    @DurationDays  INT,
    @StartDate     DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @StartDate IS NULL
        SET @StartDate = CAST(GETDATE() AS DATE);

    -- Customer must belong to the pharmacist
    IF NOT EXISTS (
        SELECT 1 FROM dbo.Customers
        WHERE CustomerID = @CustomerID AND PharmacistID = @PharmacistID
    )
    BEGIN
        RAISERROR(N'Customer is not assigned to this pharmacist.', 16, 1);
        RETURN;
    END

    DECLARE @ToDeduct DECIMAL(18,2) =
        CAST(@TimesPerDay AS DECIMAL(18,2)) * @UnitsPerDose * @DurationDays;

    BEGIN TRY
        BEGIN TRAN;

        -- Deduct inventory (must be enough)
        UPDATE dbo.PharmacistMedicines
           SET QtyAvailable = QtyAvailable - @ToDeduct
         WHERE PharmacistID = @PharmacistID
           AND MedicineID   = @MedicineID
           AND QtyAvailable >= @ToDeduct;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR(N'Not enough stock for this pharmacist.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        -- Save prescription
        INSERT INTO dbo.CustomerMedicines
            (CustomerID, MedicineID, TimesPerDay, UnitsPerDose, StartDate, DurationDays, PrescribedByPharmacistID)
        VALUES
            (@CustomerID, @MedicineID, @TimesPerDay, @UnitsPerDose, @StartDate, @DurationDays, @PharmacistID);

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

----------------------------------------------------------
-- 6) Optional safety tables (ingredients / allergies / interactions)
----------------------------------------------------------

/* Ingredients */
IF OBJECT_ID('dbo.Ingredients','U') IS NULL
BEGIN
    CREATE TABLE dbo.Ingredients(
        IngredientID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL UNIQUE
    );
END
GO

/* MedicineIngredients (many-to-many) */
IF OBJECT_ID('dbo.MedicineIngredients','U') IS NULL
BEGIN
    CREATE TABLE dbo.MedicineIngredients(
        MedicineID   INT NOT NULL FOREIGN KEY REFERENCES dbo.Medicines(MedicineID),
        IngredientID INT NOT NULL FOREIGN KEY REFERENCES dbo.Ingredients(IngredientID),
        CONSTRAINT PK_MedicineIngredients PRIMARY KEY (MedicineID, IngredientID)
    );
END
GO

/* IngredientInteractions */
IF OBJECT_ID('dbo.IngredientInteractions','U') IS NULL
BEGIN
    CREATE TABLE dbo.IngredientInteractions(
        InteractionID INT IDENTITY(1,1) PRIMARY KEY,
        IngredientAID INT NOT NULL FOREIGN KEY REFERENCES dbo.Ingredients(IngredientID),
        IngredientBID INT NOT NULL FOREIGN KEY REFERENCES dbo.Ingredients(IngredientID),
        Severity VARCHAR(20) NOT NULL,  -- Minor / Moderate / Major
        Note NVARCHAR(500) NULL,
        CONSTRAINT UX_Interaction UNIQUE(IngredientAID, IngredientBID)
    );
END
GO

/* CustomerAllergies */
IF OBJECT_ID('dbo.CustomerAllergies','U') IS NULL
BEGIN
    CREATE TABLE dbo.CustomerAllergies(
        CustomerID   INT NOT NULL FOREIGN KEY REFERENCES dbo.Customers(CustomerID),
        IngredientID INT NOT NULL FOREIGN KEY REFERENCES dbo.Ingredients(IngredientID),
        Note NVARCHAR(300) NULL,
        CONSTRAINT PK_CustomerAllergies PRIMARY KEY (CustomerID, IngredientID)
    );
END
GO

/* Check prescription safety */
IF OBJECT_ID('dbo.sp_CheckPrescriptionSafety','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CheckPrescriptionSafety;
GO
CREATE PROCEDURE dbo.sp_CheckPrescriptionSafety
    @CustomerID INT,
    @MedicineID INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH NewMedIng AS (
        SELECT IngredientID
        FROM dbo.MedicineIngredients
        WHERE MedicineID = @MedicineID
    ),
    ActiveCustMeds AS (
        SELECT MedicineID
        FROM dbo.CustomerMedicines
        WHERE CustomerID = @CustomerID
          AND StartDate <= CAST(GETDATE() AS DATE)
          AND EndDate   >= CAST(GETDATE() AS DATE)
    ),
    ActiveIng AS (
        SELECT DISTINCT mi.IngredientID
        FROM ActiveCustMeds acm
        JOIN dbo.MedicineIngredients mi ON mi.MedicineID = acm.MedicineID
    )
    -- Allergies
    SELECT 'Allergy' AS IssueType,
           i.Name    AS Item1,
           NULL      AS Item2,
           'Major'   AS Severity,
           N'Patient has an allergy to this ingredient.' AS Note
    FROM dbo.CustomerAllergies ca
    JOIN dbo.Ingredients i ON i.IngredientID = ca.IngredientID
    WHERE ca.CustomerID = @CustomerID
      AND EXISTS (SELECT 1 FROM NewMedIng n WHERE n.IngredientID = ca.IngredientID)

    UNION ALL

    -- Interactions
    SELECT 'Interaction' AS IssueType,
           i1.Name AS Item1,
           i2.Name AS Item2,
           ii.Severity,
           ii.Note
    FROM NewMedIng n
    JOIN ActiveIng ai ON 1=1
    JOIN dbo.IngredientInteractions ii
      ON (ii.IngredientAID = n.IngredientID AND ii.IngredientBID = ai.IngredientID)
      OR (ii.IngredientBID = n.IngredientID AND ii.IngredientAID = ai.IngredientID)
    JOIN dbo.Ingredients i1 ON i1.IngredientID = n.IngredientID
    JOIN dbo.Ingredients i2 ON i2.IngredientID = ai.IngredientID;
END
GO
