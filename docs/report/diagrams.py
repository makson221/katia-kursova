import sys
from PIL import Image, ImageDraw, ImageFont
out = sys.argv[1]
F = "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf"
FB = "/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf"
FI = FB
SC = 2
def font(p, s): return ImageFont.truetype(p, s * SC)

def box(d, x, y, w, title, attrs, abstract=False, fill="#EFF6FF", head="#2563EB"):
    x, y, w = x*SC, y*SC, w*SC
    th = 26*SC; lh = 17*SC
    h = th + max(1, len(attrs)) * lh + 8*SC
    d.rectangle([x, y, x+w, y+h], fill=fill, outline="#1E293B", width=2*SC//2+1)
    d.rectangle([x, y, x+w, y+th], fill=head, outline="#1E293B", width=2)
    f = font(FI if abstract else FB, 13)
    t = ("«abstract» " if abstract else "") + title
    tw = d.textlength(t, font=f)
    d.text((x + (w-tw)/2, y + 5*SC), t, font=f, fill="white")
    fa = font(F, 11)
    for i, a in enumerate(attrs):
        d.text((x + 6*SC, y + th + 4*SC + i*lh), a, font=fa, fill="#0F172A")
    return (x/SC, y/SC, w/SC, h/SC)

def arrow_inherit(d, child, parent):
    cx = (child[0] + child[2]/2)*SC; cy = child[1]*SC
    px = (parent[0] + parent[2]/2)*SC; py = (parent[1] + parent[3])*SC
    my = (cy + py) / 2
    d.line([(cx, cy), (cx, my), (px, my), (px, py + 14*SC)], fill="#1E293B", width=2*SC)
    s = 9*SC
    d.polygon([(px, py), (px - s, py + 14*SC), (px + s, py + 14*SC)], fill="white", outline="#1E293B", width=2*SC)

# --- Діаграма класів ---
W, H = 1380, 640
img = Image.new("RGB", (W*SC, H*SC), "white"); d = ImageDraw.Draw(img)
iv = box(d, 570, 10, 240, "IValidatable", ["+ Validate(): IEnumerable<string>"], fill="#F1F5F9", head="#64748B")
en = box(d, 570, 110, 240, "Entity", ["+ Id: int", "+ Validate()  virtual", "+ Clone(): Entity"], abstract=True)
# realization arrow (dashed)
x = (en[0]+en[2]/2)*SC
for yy in range(int((en[1])*SC), int((iv[1]+iv[3])*SC + 14*SC), -10*SC):
    d.line([(x, yy), (x, yy - 5*SC)], fill="#1E293B", width=2*SC)
py = (iv[1]+iv[3])*SC; s = 9*SC
d.polygon([(x, py), (x-s, py+14*SC), (x+s, py+14*SC)], fill="white", outline="#1E293B", width=2*SC)

pe = box(d, 40, 290, 270, "Person", ["+ LastName, FirstName, MiddleName", "+ BirthDate, Phone, Email", "+ FullName, ShortName, Age", "+ RoleName  abstract", "# MinAge  virtual"], abstract=True)
others = [
    ("Group", ["+ Name, Specialty", "+ StartYear, Capacity", "+ MonthlyFee, CuratorId"]),
    ("Subject", ["+ Name, Hours", "+ ControlForm, TeacherId", "+ Credits"]),
    ("Grade", ["+ StudentId, SubjectId", "+ Score, Type, Date", "+ Ects, National"]),
    ("Payment", ["+ StudentId, Amount", "+ Date, Method", "+ ReceiptNumber"]),
    ("Lesson", ["+ GroupId, SubjectId", "+ TeacherId, Day", "+ Number, Room"]),
    ("User", ["+ Login, PasswordHash", "+ Role, TeacherId", "+ IsActive"]),
]
xs = 335
for i, (n, a) in enumerate(others):
    b = box(d, xs + i*172, 290, 164, n, a)
    arrow_inherit(d, b, en)
arrow_inherit(d, pe, en)
st = box(d, 0, 490, 180, "Student", ["+ GroupId, ContractNumber", "+ Status, DiscountPercent", "+ EnrollmentDate, EndDate", "+ RoleName = \"Студент\""])
te = box(d, 195, 490, 180, "Teacher", ["+ Position, AcademicDegree", "+ Specialization", "+ HireDate, HourlyRate", "+ RoleName = \"Викладач\""])
arrow_inherit(d, st, pe); arrow_inherit(d, te, pe)
img = img.resize((W*SC//1, H*SC//1))
img.save(f"{out}/class-diagram.png")

# --- Архітектура ---
W, H = 1100, 560
img = Image.new("RGB", (W*SC, H*SC), "white"); d = ImageDraw.Draw(img)
def layer(y, h, title, color, items, x=20, w=1060):
    d.rounded_rectangle([x*SC, y*SC, (x+w)*SC, (y+h)*SC], radius=10*SC, fill=color, outline="#334155", width=2*SC)
    d.text(((x+14)*SC, (y+8)*SC), title, font=font(FB, 15), fill="#0F172A")
    n = len(items); iw = (w - 28 - (n-1)*12) / n
    for i, it in enumerate(items):
        ix = x + 14 + i*(iw+12)
        d.rounded_rectangle([ix*SC, (y+38)*SC, (ix+iw)*SC, (y+h-12)*SC], radius=6*SC, fill="white", outline="#94A3B8", width=SC)
        lines = it.split("\n")
        for j, ln in enumerate(lines):
            f = font(FB if j == 0 else F, 12 if j == 0 else 11)
            tw = d.textlength(ln, font=f)
            d.text(((ix + iw/2)*SC - tw/2, (y + 46 + j*17)*SC), ln, font=f, fill="#0F172A")
def down(y):
    x = 550*SC
    d.line([(x, y*SC), (x, (y+18)*SC)], fill="#334155", width=3*SC)
    d.polygon([(x-8*SC, (y+12)*SC), (x+8*SC, (y+12)*SC), (x, (y+22)*SC)], fill="#334155")
layer(10, 110, "Рівень представлення — PrivateSchool.WinForms", "#DBEAFE",
      ["Форми\nLoginForm, MainForm\nStudentCardForm", "Розділи\nViewBase, CrudView<T>\n10 розділів", "Діалоги\nEntityDialog<T>\n8 діалогів", "Елементи керування\nFlatButton, StatCard\nBarChart, StyledGrid"])
down(122)
layer(146, 110, "Фасад — SchoolSystem", "#EDE9FE", ["SchoolSystem\nєдина точка доступу UI до логіки"])
down(258)
layer(282, 120, "Бізнес-логіка — PrivateSchool.Core / Services, Reports", "#DCFCE7",
      ["AuthService\nролі, паролі", "FinanceService\nнарахування, борги", "AcademicService\nбали, рейтинги", "ScheduleService\nконфлікти", "IntegrityService\nцілісність", "Report ×6\nCSV / HTML"])
down(404)
layer(428, 120, "Доступ до даних — PrivateSchool.Core / Data, Models", "#FEF3C7",
      ["Моделі\nEntity, Person, Student…", "Repository<T>\nIRepository<T>", "SchoolDatabase\nконтекст даних", "IDataStorage\nJsonDataStorage → school.json"])
img.save(f"{out}/architecture.png")
print("ok")
