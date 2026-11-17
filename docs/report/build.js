// Генератор звіту курсового проєкту (docx).  node build.js <out.docx> [toc.json]
const fs = require("fs");
const path = require("path");
const {
  Document, Packer, Paragraph, TextRun, ImageRun, Table, TableRow, TableCell, WidthType,
  AlignmentType, HeadingLevel, BorderStyle, ShadingType, LevelFormat, Header, PageNumber,
  PositionalTab, PositionalTabAlignment, PositionalTabLeader, PositionalTabRelativeTo, TabStopType,
} = require("docx");

const OUT = process.argv[2];
const TOC = process.argv[3] && fs.existsSync(process.argv[3]) ? JSON.parse(fs.readFileSync(process.argv[3], "utf8")) : {};
const REPO = "/home/user/katia-kursova";
const SHOT = (n) => path.join(REPO, "docs/screenshots", n);
const DIAG = (n) => path.join(REPO, "docs/report", n);

const FONT = "Times New Roman";
const SZ = 28;            // 14 pt
const LINE = 360;         // 1,5 інтервалу
const INDENT = 709;       // абзацний відступ 1,25 см
const TEXT_W = 9355;      // 165 мм (А4 мінус поля 30 + 15 мм)

// ---------- допоміжні функції ----------
function runs(text, base = {}) {
  // **жирний**, `код`
  const parts = text.split(/(\*\*[^*]+\*\*|`[^`]+`)/).filter(Boolean);
  return parts.map((p) => {
    if (p.startsWith("**")) return new TextRun({ text: p.slice(2, -2), bold: true, ...base });
    if (p.startsWith("`")) return new TextRun({ text: p.slice(1, -1), font: "Courier New", size: base.size ? base.size - 2 : SZ - 2 });
    return new TextRun({ text: p, ...base });
  });
}

const P = (text, opts = {}) => new Paragraph({
  children: runs(text),
  alignment: opts.align ?? AlignmentType.JUSTIFIED,
  indent: opts.noIndent ? undefined : { firstLine: INDENT },
  spacing: { line: LINE, before: 0, after: 0 },
  keepNext: opts.keepNext,
});

const H1 = (text) => new Paragraph({
  heading: HeadingLevel.HEADING_1,
  pageBreakBefore: true,
  alignment: AlignmentType.CENTER,
  children: [new TextRun({ text })],
});

const H2 = (text) => new Paragraph({
  heading: HeadingLevel.HEADING_2,
  indent: { firstLine: INDENT },
  children: [new TextRun({ text })],
});

const bullets = (items) => items.map((t) => new Paragraph({
  numbering: { reference: "dash", level: 0 },
  children: runs(t),
  alignment: AlignmentType.JUSTIFIED,
  spacing: { line: LINE, before: 0, after: 0 },
}));

const numbered = (items, ref) => items.map((t) => new Paragraph({
  numbering: { reference: ref, level: 0 },
  children: runs(t),
  alignment: AlignmentType.JUSTIFIED,
  spacing: { line: LINE, before: 0, after: 0 },
}));

function pngSize(file) {
  const b = fs.readFileSync(file);
  return { w: b.readUInt32BE(16), h: b.readUInt32BE(20) };
}

let figNo = {};
function figure(file, caption, chapter, maxW = 600, maxH = 560) {
  figNo[chapter] = (figNo[chapter] || 0) + 1;
  const n = `${chapter}.${figNo[chapter]}`;
  const { w, h } = pngSize(file);
  let width = maxW, height = Math.round(h * maxW / w);
  if (height > maxH) { height = maxH; width = Math.round(w * maxH / h); }
  return {
    n,
    items: [
      new Paragraph({
        alignment: AlignmentType.CENTER,
        keepNext: true,
        spacing: { before: 120, after: 60 },
        children: [new ImageRun({ type: "png", data: fs.readFileSync(file), transformation: { width, height } })],
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { before: 0, after: 200, line: LINE },
        children: [new TextRun({ text: `Рисунок ${n} – ${caption}` })],
      }),
    ],
  };
}

let tblNo = {};
function table(chapter, title, headers, rows, widths) {
  tblNo[chapter] = (tblNo[chapter] || 0) + 1;
  const n = `${chapter}.${tblNo[chapter]}`;
  const total = widths.reduce((a, b) => a + b, 0);
  const border = { style: BorderStyle.SINGLE, size: 4, color: "000000" };
  const borders = { top: border, bottom: border, left: border, right: border };
  const cell = (text, w, head) => new TableCell({
    width: { size: w, type: WidthType.DXA },
    borders,
    shading: head ? { fill: "E7EEF8", type: ShadingType.CLEAR, color: "auto" } : undefined,
    margins: { top: 40, bottom: 40, left: 90, right: 90 },
    children: [new Paragraph({
      alignment: head ? AlignmentType.CENTER : AlignmentType.LEFT,
      spacing: { line: 240 },
      children: runs(text, { size: 24, bold: head || undefined }),
    })],
  });
  return [
    new Paragraph({
      keepNext: true,
      spacing: { before: 160, after: 60, line: LINE },
      children: [new TextRun({ text: `Таблиця ${n} – ${title}` })],
    }),
    new Table({
      width: { size: total, type: WidthType.DXA },
      columnWidths: widths,
      rows: [
        new TableRow({ tableHeader: true, children: headers.map((h, i) => cell(h, widths[i], true)) }),
        ...rows.map((r) => new TableRow({ cantSplit: true, children: r.map((c, i) => cell(c, widths[i], false)) })),
      ],
    }),
    new Paragraph({ spacing: { after: 120 }, children: [] }),
  ];
}

function code(lines, caption) {
  const border = { style: BorderStyle.SINGLE, size: 4, color: "999999", space: 4 };
  const out = [];
  if (caption) out.push(new Paragraph({
    keepNext: true, spacing: { before: 120, after: 60, line: LINE }, indent: { firstLine: INDENT },
    children: [new TextRun({ text: caption })],
  }));
  const b = { style: BorderStyle.SINGLE, size: 4, color: "999999" };
  out.push(new Table({
    width: { size: TEXT_W, type: WidthType.DXA },
    columnWidths: [TEXT_W],
    rows: [new TableRow({ cantSplit: true, children: [new TableCell({
      width: { size: TEXT_W, type: WidthType.DXA },
      borders: { top: b, bottom: b, left: b, right: b },
      shading: { fill: "F5F5F5", type: ShadingType.CLEAR, color: "auto" },
      margins: { top: 60, bottom: 60, left: 140, right: 140 },
      children: lines.map((l) => new Paragraph({
        spacing: { line: 240, before: 0, after: 0 },
        children: [new TextRun({ text: l.length ? l : " ", font: "Courier New", size: 20 })],
      })),
    })] })],
  }));
  out.push(new Paragraph({ spacing: { after: 120 }, children: [] }));
  return out;
}

const empty = (n = 1) => Array.from({ length: n }, () => new Paragraph({ spacing: { line: LINE }, children: [] }));
const center = (text, opts = {}) => new Paragraph({
  alignment: AlignmentType.CENTER,
  spacing: { line: LINE, before: 0, after: 0 },
  children: [new TextRun({ text, bold: opts.bold, size: opts.size, allCaps: opts.caps })],
});

// ---------- титульна сторінка ----------
const title = [
  center("МІНІСТЕРСТВО ОСВІТИ І НАУКИ УКРАЇНИ"),
  center("_______________________________________________"),
  center("(повна назва закладу освіти)"),
  ...empty(1),
  center("Кафедра ______________________________"),
  ...empty(5),
  center("КУРСОВИЙ ПРОЄКТ", { bold: true, size: 32 }),
  center("з дисципліни «Об'єктно-орієнтоване програмування»"),
  center("на тему:"),
  center("«Розробка автоматизованої системи приватного навчального закладу»", { bold: true }),
  ...empty(5),
  new Paragraph({ indent: { left: 4200 }, spacing: { line: LINE }, children: [new TextRun("Виконав(ла): студент(ка) групи ______")] }),
  new Paragraph({ indent: { left: 4200 }, spacing: { line: LINE }, children: [new TextRun("__________________________________")] }),
  new Paragraph({ indent: { left: 4200 }, spacing: { line: LINE }, children: [new TextRun("Керівник: _________________________")] }),
  new Paragraph({ indent: { left: 4200 }, spacing: { line: LINE }, children: [new TextRun("Оцінка: ______   Підпис: ________")] }),
  ...empty(5),
  center("Місто – 2026"),
];

// ---------- зміст ----------
const tocEntries = [
  ["ВСТУП", 0], ["1 АНАЛІЗ ПРЕДМЕТНОЇ ОБЛАСТІ ТА ПОСТАНОВКА ЗАДАЧІ", 0],
  ["1.1 Опис предметної області", 1], ["1.2 Постановка задачі та вимоги до системи", 1],
  ["2 ПРОЄКТУВАННЯ СИСТЕМИ", 0], ["2.1 Архітектура застосунку", 1], ["2.2 Ієрархія класів предметної області", 1],
  ["2.3 Сервіси бізнес-логіки та шаблони проєктування", 1],
  ["3 ПРОГРАМНА РЕАЛІЗАЦІЯ", 0], ["3.1 Реалізація принципів ООП", 1], ["3.2 Основні алгоритми", 1],
  ["3.3 Зберігання даних і безпека", 1],
  ["4 ОПИС РОБОТИ ПРОГРАМИ", 0], ["4.1 Вхід до системи та головна панель", 1], ["4.2 Робота з довідниками", 1],
  ["4.3 Успішність і оплати", 1], ["4.4 Розклад занять", 1], ["4.5 Звіти та робота під роллю викладача", 1],
  ["5 ТЕСТУВАННЯ", 0], ["ВИСНОВКИ", 0], ["ПЕРЕЛІК ДЖЕРЕЛ", 0],
];
const toc = [
  new Paragraph({ pageBreakBefore: true, alignment: AlignmentType.CENTER, spacing: { after: 240, line: LINE }, children: [new TextRun({ text: "ЗМІСТ", bold: true })] }),
  ...tocEntries.map(([t, lvl]) => new Paragraph({
    indent: { left: lvl ? 400 : 0 },
    spacing: { line: LINE },
    tabStops: [{ type: TabStopType.RIGHT, position: TEXT_W, leader: "dot" }],
    children: [new TextRun({ text: `${t}\t${TOC[t] ?? ""}` })],
  })),
];

// ---------- ВСТУП ----------
const intro = [
  H1("ВСТУП"),
  P("Приватні заклади освіти щодня працюють із великим обсягом інформації: контингент здобувачів освіти, договори про навчання та оплати за ними, кадровий склад, навчальні плани, розклад занять і результати оцінювання. Ведення цих даних у паперових журналах або розрізнених електронних таблицях призводить до помилок, дублювання та ускладнює підготовку звітності. Особливістю саме приватного закладу є фінансова складова: кожен студент навчається за договором, тому потрібен постійний контроль нарахувань, надходжень і заборгованості."),
  P("**Актуальність** роботи полягає у створенні єдиної інформаційної системи, яка автоматизує основні процеси закладу та надає керівництву оперативну аналітику."),
  P("**Мета роботи** – розробити настільний застосунок «EduManager» для автоматизації діяльності приватного навчального закладу із застосуванням принципів об'єктно-орієнтованого програмування мовою C#."),
  P("Для досягнення мети поставлено такі **завдання**:", { keepNext: true }),
  ...numbered([
    "проаналізувати предметну область і визначити вимоги до системи;",
    "спроєктувати архітектуру застосунку та ієрархію класів;",
    "реалізувати зберігання даних, бізнес-логіку та графічний інтерфейс Windows Forms;",
    "розмежувати доступ користувачів за ролями;",
    "реалізувати формування звітів, їх друк та експорт;",
    "перевірити коректність роботи модульними тестами.",
  ], "tasks"),
  P("**Об'єкт дослідження** – процеси обліку навчальної та фінансової діяльності приватного закладу освіти. **Предмет дослідження** – методи об'єктно-орієнтованого проєктування й програмування інформаційних систем на платформі .NET."),
  P("Програму розроблено мовою C# на платформі .NET 8 з використанням бібліотеки Windows Forms; для модульного тестування використано фреймворк xUnit."),
];

// ---------- РОЗДІЛ 1 ----------
const ch1 = [
  H1("1 АНАЛІЗ ПРЕДМЕТНОЇ ОБЛАСТІ ТА ПОСТАНОВКА ЗАДАЧІ"),
  H2("1.1 Опис предметної області"),
  P("Приватний коледж готує здобувачів освіти за кількома спеціальностями. Студенти об'єднуються в академічні групи; кожна група має спеціальність, рік набору, місткість, щомісячну вартість навчання та куратора. З кожним студентом укладається договір із номером, датою зарахування та, можливо, знижкою. Плата вноситься щомісяця з вересня по червень."),
  P("Викладачі ведуть дисципліни навчального плану. Дисципліна має обсяг у годинах (кредитах ЄКТС) і форму підсумкового контролю – іспит або залік. Заняття проводяться за тижневим розкладом: шість пар на день з понеділка по суботу, в певних аудиторіях. Успішність оцінюється за 100-бальною шкалою з переведенням у шкалу ЄКТС та національну."),
  P("Із системою працюють три категорії користувачів, права яких наведено в таблиці 1.1.", { keepNext: true }),
  ...table(1, "Ролі користувачів системи", ["Роль", "Доступ"], [
    ["Адміністратор", "Усі розділи, керування обліковими записами користувачів"],
    ["Менеджер / бухгалтер", "Студенти, групи, оплати, розклад, звіти"],
    ["Викладач", "Перегляд студентів і розкладу, журнал оцінок лише своїх дисциплін, звіти з успішності"],
  ], [2800, 6555]),
  H2("1.2 Постановка задачі та вимоги до системи"),
  P("Необхідно розробити настільний застосунок, що автоматизує облік діяльності закладу. Основні функціональні вимоги:", { keepNext: true }),
  ...bullets([
    "автентифікація користувачів і розмежування доступу за ролями;",
    "ведення довідників студентів, викладачів, груп, дисциплін і користувачів з додаванням, редагуванням, видаленням, пошуком і фільтрацією;",
    "журнал успішності з автоматичним переведенням балів у шкали ЄКТС та національну;",
    "облік оплат, розрахунок нарахувань, сальдо та заборгованості кожного студента;",
    "складання розкладу з контролем накладок групи, викладача та аудиторії;",
    "формування звітів, їх друк і експорт у формати CSV та HTML;",
    "головна панель з ключовими показниками та діаграмою;",
    "збереження даних між сеансами роботи та резервне копіювання.",
  ]),
  P("Нефункціональні вимоги: робота під Windows 10/11 на платформі .NET 8; перевірка всіх введених даних з інформативними повідомленнями; збереження цілісності пов'язаних даних; безпечне зберігання паролів; відокремлення бізнес-логіки від інтерфейсу для можливості автоматичного тестування."),
];

// ---------- РОЗДІЛ 2 ----------
const fArch = figure(DIAG("architecture.png"), "Архітектура застосунку", 2, 600);
const fClass = figure(DIAG("class-diagram.png"), "Діаграма класів предметної області", 2, 620);
const ch2 = [
  H1("2 ПРОЄКТУВАННЯ СИСТЕМИ"),
  H2("2.1 Архітектура застосунку"),
  P("Застосунок побудовано за багатошаровою архітектурою. Рішення складається з трьох проєктів: бібліотеки класів `PrivateSchool.Core` (моделі, доступ до даних, бізнес-логіка, звіти), проєкту інтерфейсу `PrivateSchool.WinForms` та проєкту модульних тестів `PrivateSchool.Tests`."),
  P(`Інтерфейс користувача звертається до бізнес-логіки лише через фасад \`SchoolSystem\`, який створює і зв'язує між собою контекст даних та сервіси (рисунок ${fArch.n}). Завдяки цьому логіка не залежить від Windows Forms і може перевірятися автоматичними тестами або використовуватися в іншому інтерфейсі.`),
  ...fArch.items,
  H2("2.2 Ієрархія класів предметної області"),
  P(`Базовим є абстрактний клас \`Entity\`, що реалізує інтерфейс \`IValidatable\`. Він містить ідентифікатор, віртуальний метод перевірки даних \`Validate()\` і метод клонування \`Clone()\`. Від нього успадковано абстрактний клас \`Person\` (ПІБ, дата народження, контакти, обчислювані властивості \`FullName\`, \`Age\`, абстрактна властивість \`RoleName\`), від якого походять класи \`Student\` і \`Teacher\`, а також класи \`Group\`, \`Subject\`, \`Grade\`, \`Payment\`, \`Lesson\` і \`User\` (рисунок ${fClass.n}).`),
  ...fClass.items,
  P("Зв'язки між сутностями задаються ідентифікаторами (`GroupId`, `StudentId`, `SubjectId`, `TeacherId`) аналогічно до зовнішніх ключів реляційної бази даних. Кожен клас-нащадок перевизначає метод `Validate()`, спочатку викликаючи базову реалізацію, а потім додаючи власні правила."),
  H2("2.3 Сервіси бізнес-логіки та шаблони проєктування"),
  P("Бізнес-логіку розподілено між окремими сервісами, кожен з яких має одну відповідальність (таблиця 2.1).", { keepNext: true }),
  ...table(2, "Сервіси бізнес-логіки", ["Клас", "Відповідальність"], [
    ["AuthService", "Вхід і вихід, ролі та дозволи, створення користувачів, зміна паролів"],
    ["FinanceService", "Щомісячні нарахування, сальдо, боржники, надходження за період, номери квитанцій"],
    ["AcademicService", "Середні бали, рейтинг групи, показники успішності груп"],
    ["ScheduleService", "Пошук конфліктів у розкладі, розклад групи та викладача, навантаження"],
    ["IntegrityService", "Перевірка можливості видалення та каскадне видалення залежних записів"],
    ["Report (6 нащадків)", "Побудова звітів; експорт через CsvReportExporter і HtmlReportExporter"],
  ], [2800, 6555]),
  P("Під час проєктування застосовано шаблони, наведені в таблиці 2.2.", { keepNext: true }),
  ...table(2, "Використані шаблони проєктування", ["Шаблон", "Застосування"], [
    ["Repository", "IRepository<T> і Repository<T> – єдиний спосіб роботи з колекціями сутностей"],
    ["Facade", "SchoolSystem – одна точка доступу інтерфейсу до всіх сервісів"],
    ["Strategy", "IDataStorage (JSON-файл / пам'ять), IPasswordHasher, IReportExporter (CSV / HTML)"],
    ["Template Method", "Report.Build() викликає абстрактний BuildTable(); EntityDialog<T> визначає порядок «створити поля – заповнити – зберегти»"],
    ["Prototype", "Entity.Clone() – редагування виконується над копією, тому скасування не змінює дані"],
    ["Observer", "Події Repository.Changed і SchoolDatabase.DataChanged – автозбереження та оновлення рядка стану"],
  ], [2400, 6955]),
];

// ---------- РОЗДІЛ 3 ----------
const ch3 = [
  H1("3 ПРОГРАМНА РЕАЛІЗАЦІЯ"),
  H2("3.1 Реалізація принципів ООП"),
  P("**Інкапсуляція.** Колекція записів репозиторію є закритою, змінити її можна лише методами `Add`, `Update` і `Remove`, які перед збереженням перевіряють коректність даних. Обчислювані дані (ПІБ, вік, курс групи, оцінка ЄКТС) реалізовано властивостями лише для читання.", { keepNext: true }),
  ...code([
    "public class Repository<T> : IRepository<T> where T : Entity",
    "{",
    "    private readonly List<T> _items = new();",
    "    private int _nextId = 1;",
    "",
    "    public T Add(T item)",
    "    {",
    "        EnsureValid(item);      // ValidationException зі списком помилок",
    "        item.Id = _nextId++;",
    "        _items.Add(item);",
    "        OnChanged();            // подія для автозбереження",
    "        return item;",
    "    }",
    "}",
  ], "Лістинг 3.1 – Узагальнений репозиторій"),
  P("**Наслідування та поліморфізм.** Правила перевірки даних розширюються на кожному рівні ієрархії: клас `Person` перевіряє ПІБ і вік, а клас `Student` додає власні правила та перевизначає віртуальну властивість мінімального віку.", { keepNext: true }),
  ...code([
    "public class Student : Person",
    "{",
    "    public override string RoleName => \"Студент\";",
    "    protected override int MinAge => 6;",
    "",
    "    public override IEnumerable<string> Validate()",
    "    {",
    "        foreach (var error in base.Validate()) yield return error;",
    "        if (GroupId <= 0) yield return \"Оберіть групу.\";",
    "        if (DiscountPercent is < 0 or > 100)",
    "            yield return \"Знижка має бути в межах 0–100 %.\";",
    "    }",
    "}",
  ], "Лістинг 3.2 – Перевизначення методу перевірки в класі Student"),
  P("**Абстракція.** Абстрактний клас `Report` оголошує лише контракт звіту, а алгоритм побудови таблиці визначають шість класів-нащадків. Інтерфейси `IDataStorage`, `IPasswordHasher` і `IReportExporter` дозволяють підміняти реалізації: наприклад, у тестах використовується сховище в оперативній пам'яті."),
  P("**Узагальнене програмування.** Один клас `CrudView<T>` реалізує спільну поведінку шести табличних розділів (панель інструментів, пошук, фільтри, гарячі клавіші, експорт), а `EntityDialog<T>` – восьми діалогів редагування. Власні елементи керування (`FlatButton`, `StatCard`, `BarChart`, `StyledGrid`) успадковано від стандартних класів Windows Forms з перевизначенням методу `OnPaint`."),
  H2("3.2 Основні алгоритми"),
  P("**Розрахунок заборгованості.** Кількість оплачуваних місяців рахується від місяця зарахування до поточного (або до дати відрахування), окрім липня та серпня. Нараховано = вартість групи × (1 – знижка) × кількість місяців; сальдо = сума оплат – нараховано. Від'ємне сальдо означає заборгованість."),
  P("**Переведення оцінок.** Бал за 100-бальною шкалою переводиться у шкалу ЄКТС виразом `switch` із шаблонами відношень:", { keepNext: true }),
  ...code([
    "public static string ToEcts(int score) => score switch",
    "{",
    "    >= 90 => \"A\",  >= 82 => \"B\",  >= 74 => \"C\",",
    "    >= 64 => \"D\",  >= 60 => \"E\",  >= 35 => \"FX\",",
    "    _ => \"F\"",
    "};",
  ], "Лістинг 3.3 – Переведення балів у шкалу ЄКТС"),
  P("**Перевірка розкладу.** Для заняття вибираються всі інші заняття в той самий день і пару; конфліктом вважається збіг групи, викладача або аудиторії. Перевірка виконується в діалозі в реальному часі та повторно перед збереженням."),
  P("**Цілісність даних.** Сервіс `IntegrityService` визначає, чи можна видалити запис: не можна видалити групу зі студентами, викладача із заняттями в розкладі, дисципліну з виставленими оцінками. Видалення студента після підтвердження каскадно видаляє його оцінки та оплати в межах однієї операції."),
  H2("3.3 Зберігання даних і безпека"),
  P("Дані зберігаються у файлі `Data/school.json` і записуються автоматично після кожної зміни (атомарно, з резервною копією попередньої версії). Під час першого запуску створюється демонстраційна база: 5 груп, 70 студентів, 10 викладачів, 13 дисциплін, розклад, оцінки та оплати."),
  P("Паролі зберігаються як хеш PBKDF2 (SHA-256) із випадковою сіллю; після трьох невдалих спроб вхід блокується на 15 секунд."),
];

// ---------- РОЗДІЛ 4 ----------
const f = {
  login: figure(SHOT("01-login.png"), "Вікно входу до системи", 4, 520),
  dash: figure(SHOT("02-dashboard.png"), "Головна панель (роль «Адміністратор»)", 4),
  students: figure(SHOT("03-students.png"), "Розділ «Студенти»", 4),
  sdialog: figure(SHOT("07-student-dialog.png"), "Діалог редагування студента", 4, 480, 500),
  card: figure(SHOT("08-student-card.png"), "Картка студента", 4, 560),
  grades: figure(SHOT("04-grades.png"), "Журнал успішності", 4),
  pay: figure(SHOT("09-payment-dialog.png"), "Додавання оплати", 4, 480),
  sched: figure(SHOT("05-schedule.png"), "Розклад занять групи", 4),
  lesson: figure(SHOT("10-lesson-dialog.png"), "Редагування заняття з перевіркою накладок", 4, 480),
  report: figure(SHOT("06-reports.png"), "Звіт «Рейтинг студентів групи»", 4),
  teacher: figure(SHOT("11-teacher-dashboard.png"), "Головна панель під роллю «Викладач»", 4),
};
const ch4 = [
  H1("4 ОПИС РОБОТИ ПРОГРАМИ"),
  H2("4.1 Вхід до системи та головна панель"),
  P(`Після запуску програми відкривається вікно входу (рисунок ${f.login.n}). Користувач вводить логін і пароль; для демонстрації створено облікові записи \`admin\`/\`admin123\`, \`manager\`/\`manager123\` і \`teacher\`/\`teacher123\`.`),
  ...f.login.items,
  P(`Головне вікно містить бічну панель навігації, меню та рядок стану. Набір розділів залежить від ролі користувача. Головна панель (рисунок ${f.dash.n}) показує кількість студентів і викладачів, середній бал, надходження за місяць, загальну заборгованість, діаграму надходжень за шість місяців, розклад на сьогодні та найбільших боржників.`),
  ...f.dash.items,
  H2("4.2 Робота з довідниками"),
  P(`Усі довідники мають однаковий вигляд (рисунок ${f.students.n}): кнопки «Додати», «Редагувати», «Видалити», «Експорт», поле пошуку, рядок фільтрів і таблицю з сортуванням за будь-яким стовпцем. Під таблицею виводяться підсумки. У розділі «Студенти» стовпець «Сальдо» підсвічує боржників червоним, а відраховані студенти показані сірим.`),
  ...f.students.items,
  P(`Додавання та редагування виконуються в діалогових вікнах (рисунок ${f.sdialog.n}). Обов'язкові поля позначено зірочкою. Під час збереження дані перевіряються, і в разі помилок діалог не закривається, а під полями з'являється перелік того, що потрібно виправити. Для студента одразу розраховується вартість навчання з урахуванням знижки.`),
  ...f.sdialog.items,
  P(`Кнопка «Картка» відкриває особову картку студента (рисунок ${f.card.n}) з вкладками «Успішність», «Розрахунки» та «Особисті дані». На вкладці успішності показано всі оцінки, середній бал загалом і за кожною дисципліною.`),
  ...f.card.items,
  H2("4.3 Успішність і оплати"),
  P(`Журнал успішності (рисунок ${f.grades.n}) містить усі оцінки з фільтрами за групою, дисципліною та видом контролю. Бали автоматично переводяться у шкали ЄКТС та національну; незадовільні оцінки виділено червоним, відмінні – зеленим.`),
  ...f.grades.items,
  P(`Під час додавання оплати (рисунок ${f.pay.n}) студента можна знайти, почавши вводити прізвище. Система показує його поточний стан розрахунків, автоматично пропонує номер квитанції та дозволяє одним кліком підставити місячну плату або суму боргу.`),
  ...f.pay.items,
  H2("4.4 Розклад занять"),
  P(`Розклад відображається у вигляді сітки «пари × дні тижня» для обраної групи або викладача (рисунок ${f.sched.n}). Колір клітинки позначає тип заняття: лекція, практичне чи лабораторне. Подвійний клік по порожній клітинці додає заняття, по заповненій – відкриває його для редагування.`),
  ...f.sched.items,
  P(`У діалозі заняття (рисунок ${f.lesson.n}) під час зміни будь-якого поля одразу перевіряються накладки: чи вільні в цей час група, викладач та аудиторія. Заняття з конфліктом зберегти неможливо.`),
  ...f.lesson.items,
  H2("4.5 Звіти та робота під роллю викладача"),
  P(`У розділі «Звіти» доступні шість звітів: заборгованість за навчання, реєстр оплат за період, рейтинг студентів групи, успішність за групами, навантаження викладачів і контингент студентів. Після вибору звіту та параметрів результат показується в таблиці з підсумками (рисунок ${f.report.n}). Звіт можна переглянути перед друком і надрукувати або зберегти у файл CSV для Excel чи HTML.`),
  ...f.report.items,
  P(`Під роллю «Викладач» доступні лише розділи «Головна», «Студенти» (перегляд), «Успішність», «Розклад» і «Звіти» (рисунок ${f.teacher.n}). Викладач бачить і змінює оцінки лише зі своїх дисциплін, на головній панелі замість фінансових показників відображаються середні бали груп і найкращі студенти.`),
  ...f.teacher.items,
];

// ---------- РОЗДІЛ 5 ----------
const ch5 = [
  H1("5 ТЕСТУВАННЯ"),
  P("Для автоматичної перевірки бізнес-логіки створено проєкт `PrivateSchool.Tests`, що містить 34 модульні тести на основі фреймворку xUnit (таблиця 5.1). Тести працюють без графічного інтерфейсу, використовуючи сховище даних в оперативній пам'яті.", { keepNext: true }),
  ...table(5, "Модульні тести", ["Група тестів", "Що перевіряється"], [
    ["ModelTests", "Шкали ЄКТС і національна, ПІБ та ініціали, перевірка даних студента, клонування"],
    ["RepositoryTests", "Нумерація записів, відхилення некоректних даних, збереження та завантаження JSON"],
    ["FinanceTests", "Пропуск літніх місяців, зупинка нарахувань після відрахування, знижки, номери квитанцій"],
    ["ScheduleTests", "Виявлення конфліктів групи, викладача й аудиторії, тижневе навантаження"],
    ["AcademicTests", "Рейтинг групи, відсоток успішності, кількість відмінників"],
    ["AuthTests", "Вхід, права ролей, складність пароля, унікальність логіна, захист останнього адміністратора"],
    ["IntegrityTests", "Заборона видалення групи зі студентами, каскадне видалення, очищення посилань"],
    ["SeedAndReportTests", "Коректність демонстраційних даних, побудова та експорт усіх звітів"],
  ], [2700, 6655]),
  P("Результат запуску командою `dotnet test`: усі 34 тести пройдено успішно. Збірка рішення виконується без помилок і попереджень компілятора."),
  P("Ручне тестування інтерфейсу охоплювало введення некоректних даних (порожні обов'язкові поля, неправильний формат телефону чи e-mail, дата народження в майбутньому, нульова сума оплати), спроби створити накладку в розкладі, видалення пов'язаних записів, повторне використання номера договору чи квитанції, а також роботу під кожною з трьох ролей. У всіх випадках програма виводила зрозумілі повідомлення й не допускала збереження некоректних даних."),
];

// ---------- ВИСНОВКИ, ДЖЕРЕЛА ----------
const concl = [
  H1("ВИСНОВКИ"),
  P("У ході виконання курсового проєкту розроблено автоматизовану систему приватного навчального закладу «EduManager». Проаналізовано предметну область і сформульовано вимоги до системи, спроєктовано багатошарову архітектуру та ієрархію класів з абстрактними базовими класами `Entity` і `Person`."),
  P("У програмі застосовано всі основні принципи об'єктно-орієнтованого програмування – інкапсуляцію, наслідування, поліморфізм та абстракцію, – а також узагальнені класи та шаблони проєктування Repository, Facade, Strategy, Template Method, Prototype і Observer. Бізнес-логіку відокремлено від інтерфейсу та покрито 34 модульними тестами."),
  P("Система автоматизує облік контингенту, кадрів, навчального плану, успішності, оплат і розкладу, контролює коректність і цілісність даних, розмежовує доступ за ролями та формує звіти з можливістю друку й експорту."),
  P("Подальший розвиток системи може включати перехід на СУБД (SQLite або SQL Server) через нову реалізацію інтерфейсу `IDataStorage`, облік відвідуваності, формування договорів і квитанцій у форматі PDF та веб-кабінет для студентів і батьків."),
];
const refs = [
  H1("ПЕРЕЛІК ДЖЕРЕЛ"),
  ...numbered([
    "Документація мови C#. Microsoft Learn. URL: https://learn.microsoft.com/dotnet/csharp/ (дата звернення: 15.11.2026).",
    "Windows Forms для .NET. Microsoft Learn. URL: https://learn.microsoft.com/dotnet/desktop/winforms/ (дата звернення: 15.11.2026).",
    "Троелсен Е., Джепікс Ф. Мова програмування C# і платформа .NET. 11-те вид. Apress.",
    "Гамма Е., Хелм Р., Джонсон Р., Вліссідес Дж. Прийоми об'єктно-орієнтованого проєктування. Патерни проєктування.",
    "Мартін Р. Чистий код. Створення, аналіз і рефакторинг.",
    "xUnit.net documentation. URL: https://xunit.net/ (дата звернення: 15.11.2026).",
    "Про освіту: Закон України від 05.09.2017 № 2145-VIII.",
  ], "refs"),
];

// ---------- документ ----------
const numberingConfig = (ref, format, text) => ({
  reference: ref,
  levels: [{ level: 0, format, text, alignment: AlignmentType.LEFT,
    style: { paragraph: { indent: { left: INDENT + 360, hanging: 360 } } } }],
});

const doc = new Document({
  creator: "EduManager",
  title: "Курсовий проєкт – EduManager",
  styles: {
    default: { document: { run: { font: FONT, size: SZ } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: SZ, bold: true },
        paragraph: { spacing: { before: 0, after: 240, line: LINE }, keepNext: true, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { font: FONT, size: SZ, bold: true },
        paragraph: { spacing: { before: 240, after: 120, line: LINE }, keepNext: true, outlineLevel: 1 } },
    ],
  },
  numbering: {
    config: [
      numberingConfig("dash", LevelFormat.BULLET, "–"),
      numberingConfig("tasks", LevelFormat.DECIMAL, "%1)"),
      numberingConfig("refs", LevelFormat.DECIMAL, "%1."),
    ],
  },
  sections: [{
    properties: {
      titlePage: true,
      page: { size: { width: 11906, height: 16838 }, margin: { top: 1134, bottom: 1134, left: 1701, right: 850, header: 567 } },
    },
    headers: {
      default: new Header({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ children: [PageNumber.CURRENT] })] })] }),
      first: new Header({ children: [new Paragraph({ children: [] })] }),
    },
    children: [
      ...title,
      ...toc,
      ...intro, ...ch1, ...ch2, ...ch3, ...ch4, ...ch5, ...concl, ...refs,
    ],
  }],
});

Packer.toBuffer(doc).then((buf) => { fs.writeFileSync(OUT, buf); console.log("written", OUT); });
