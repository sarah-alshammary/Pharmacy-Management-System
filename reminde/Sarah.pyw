import smtplib
from email.message import EmailMessage
import schedule
import time
from datetime import datetime
import tkinter as tk
from tkinter import messagebox

# ---------- EMAIL SETTINGS ----------
SENDER_EMAIL = "sarahalshammary252002@gmail.com"
SENDER_PASSWORD = "fckcesiezsvcatkt"
PATIENT_EMAIL = "sarahalshammary3@gmail.com"

EMAIL_SUBJECT = "Medication Reminder"
EMAIL_BODY = """
Hello,

This is a friendly reminder to take your medication at this time.

Best regards.
"""


def send_med_reminder():
    """Send a single reminder email."""
    try:
        msg = EmailMessage()
        msg["Subject"] = EMAIL_SUBJECT
        msg["From"] = SENDER_EMAIL
        msg["To"] = PATIENT_EMAIL
        msg.set_content(EMAIL_BODY)

        with smtplib.SMTP_SSL("smtp.gmail.com", 465) as smtp:
            smtp.login(SENDER_EMAIL, SENDER_PASSWORD)
            smtp.send_message(msg)

        print(f"[{datetime.now()}] Reminder sent to {PATIENT_EMAIL}")
    except Exception as e:
        print(f"Error sending email: {e}")


REMINDER_TIMES = []  # will be filled from GUI


def start_schedule():
    """Read times from GUI, validate them, then close window."""
    global REMINDER_TIMES
    REMINDER_TIMES = []

    for entry in time_entries:
        t = entry.get().strip()
        if not t:
            continue
        try:
            # validate HH:mm
            datetime.strptime(t, "%H:%M")
            REMINDER_TIMES.append(t)
        except ValueError:
            messagebox.showerror("Error", f"Time '{t}' is invalid. Use HH:mm format.")
            return

    if not REMINDER_TIMES:
        messagebox.showwarning("Warning", "Please enter at least one time.")
        return

    messagebox.showinfo("Info", "Reminders scheduled. Window will close and service will run in background.")
    root.destroy()


# ---------- TKINTER UI ----------
root = tk.Tk()
root.title("Medication Alerts")

tk.Label(root, text="Enter up to 4 times (HH:mm):").grid(row=0, column=0, columnspan=2, padx=10, pady=10)

time_entries = []
for i in range(4):
    tk.Label(root, text=f"Time {i+1}:").grid(row=i+1, column=0, sticky="e", padx=5, pady=5)
    e = tk.Entry(root, width=10)
    e.grid(row=i+1, column=1, sticky="w", padx=5, pady=5)
    time_entries.append(e)

btn_start = tk.Button(root, text="Start Reminders", command=start_schedule)
btn_start.grid(row=5, column=0, columnspan=2, pady=10)

root.mainloop()

# ---------- AFTER WINDOW CLOSES: START SCHEDULE ----------
if not REMINDER_TIMES:
    print("No times entered. Exiting.")
    exit()

for t in REMINDER_TIMES:
    schedule.every().day.at(t).do(send_med_reminder)
    print(f"Reminder scheduled at {t}")

print("Medication reminder system is running...")

while True:
    schedule.run_pending()
    time.sleep(1)