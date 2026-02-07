from tkinter import *
from tkintermapview import TkinterMapView
from geopy.geocoders import Nominatim
from geopy.extra.rate_limiter import RateLimiter

root = Tk()
root.geometry("1200x700")
root.title("Pharmacies")
root.configure(background="white")


titlel = Label(root, text="Pharmacies", fg="white", bg="black", font=("Tajawal", 18))
titlel.pack(fill=X, side=TOP)


left = Frame(root, bg="white", width=380)
left.pack(side=LEFT, fill=Y)
right = Frame(root, bg="white")
right.pack(side=RIGHT, fill=BOTH, expand=True)


Label(left, text="Address:", font=("Tajawal", 13), fg="black", bg="white").grid(
    row=0, column=0, padx=10, pady=(15, 5), sticky="w"
)
En = Entry(left, font=("Tajawal", 14), width=18, bd=2, relief=GROOVE)
En.grid(row=0, column=1, padx=5, pady=(15, 5), sticky="w")


map_widget = TkinterMapView(right, corner_radius=0)
map_widget.pack(fill=BOTH, expand=True, padx=10, pady=10)
map_widget.set_tile_server("https://a.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png", max_zoom=22)
map_widget.set_position(30.0, 36.0)
map_widget.set_zoom(5)


geolocator = Nominatim(user_agent="pharmacies_app_tk")
geocode = RateLimiter(geolocator.geocode, min_delay_seconds=1, swallow_exceptions=True)


current_marker = {"obj": None}


PHARMACY_NAMES = [
    "Jabal Altaj Pharmacy",
    "AlMosbah Pharmacy",
    "Marina Pharmacy",
    "Al Mqased Pharmacy",
    "Ruaa Pharmacy",
    "Vanilla Pharmacy",
    "AlRazi Pharmacy",
    "Pharma Chain Pharmacy",
    "Ard Al Salam Pharmacy",
    "Hadeel Pharmacy",
    "Ibn Sina Pharmacy",
    "Al-Radwan Pharmacy"
]


PHARMACY_COORDS = {
    "Jabal Altaj Pharmacy": None,
    "AlMosbah Pharmacy": None,
    "Marina Pharmacy": None,
    "Al Mqased Pharmacy": None,
    "Ruaa Pharmacy": None,
    "Vanilla Pharmacy": None,
    "AlRazi Pharmacy": None,
    "Pharma Chain Pharmacy": None,
    "Ard Al Salam Pharmacy": None,
    "Hadeel Pharmacy": None,
    "Ibn Sina Pharmacy": None,
    "Al-Radwan Pharmacy": None,
}


def clear_current_marker():
    """Delete the current marker if it exists"""
    if current_marker["obj"] is not None:
        try:
            current_marker["obj"].delete()
        except Exception:
            pass
        current_marker["obj"] = None

def goto_country():
    """Move the map to selected country and place a marker"""
    q = En.get().strip()
    if not q:
        return

    loc = geocode(q, language="en") or geocode(q, language="ar")
    if not loc:
        return

    clear_current_marker()  
    map_widget.set_position(loc.latitude, loc.longitude)
    map_widget.set_zoom(6)
    
    current_marker["obj"] = map_widget.set_marker(
        loc.latitude, loc.longitude, text=loc.address.split(",")[0]
    )

def goto_pharmacy(name):
    """Move to pharmacy and place only one marker"""
    
    if PHARMACY_COORDS.get(name):
        lat, lon = PHARMACY_COORDS[name]
        clear_current_marker()
        map_widget.set_position(lat, lon)
        map_widget.set_zoom(15)
        current_marker["obj"] = map_widget.set_marker(lat, lon, text=name)
        return

    
    country = En.get().strip()
    query = f"{name}, {country}" if country else name
    loc = geocode(query, language="en") or geocode(query, language="ar")
    if not loc and country:
        loc = geocode(name, language="en") or geocode(name, language="ar")
    if not loc:
        return

    clear_current_marker()
    map_widget.set_position(loc.latitude, loc.longitude)
    map_widget.set_zoom(15)
    current_marker["obj"] = map_widget.set_marker(loc.latitude, loc.longitude, text=name)


b1 = Button(
    left, text="Get Address", bg="black", fg="white",
    bd=1, relief=SOLID, width=10, cursor="hand2",
    command=goto_country
)
b1.grid(row=0, column=2, padx=8, pady=(15, 5))


start_row = 1
for i, name in enumerate(PHARMACY_NAMES):
    row = i // 3
    col = i % 3
    btn = Button(
        left, text=name, cursor="hand2",
        bg="white", fg="black", bd=1, relief=SOLID,
        font=("Tajawal", 12), width=22,
        command=lambda n=name: goto_pharmacy(n)
    )
    btn.grid(row=start_row + row, column=col, padx=10, pady=8, sticky="w")

root.mainloop()
