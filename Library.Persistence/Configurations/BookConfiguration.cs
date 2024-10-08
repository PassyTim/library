using Library.Domain;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Isbn).IsUnique();
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Constants.BookNameMaxLength);
        builder.Property(p => p.Isbn).IsRequired().HasMaxLength(Constants.BookIsbnMaxLength);
        builder.Property(p => p.Genre).IsRequired().HasMaxLength(Constants.BookGenreMaxLength);

        builder.HasData(
            new Book
            {
                Id = 1,
                Isbn = "9785171183661",
                Name = "Евгений Онегин",
                Genre = "Роман",
                Description = "Можно сказать, что весь XIX век прошёл под знаком А. С. Пушкина. Но и сегодня, в XXI веке, уже тысячекратно повторённые школьными учебниками и хрестоматиями гениальные пушкинские строки не обесценились: как всё причастное к вечности, они дарят радость, вселяют надежду и утешение. В настоящем издании представлены роман в стихах \"Евгений Онегин\", а также самые известные стихотворения А. С. Пушкина, вошедшие в сокровищницу мировой литературы.",
                ImagePath = "8fb16b40-17d4-43fa-b3fa-20238b342ad3.jpg",
                AuthorId = 1
            },
            new Book
            {
                Id = 2,
                Isbn = "9781101948294",
                Name = "The Shining",
                Genre = "Хоррор",
                Description = "\"Сияние\" – культовый роман Стивена Кинга. Роман, который и сейчас, спустя тридцать с лишним лет после триумфального выхода в свет, читается так, словно был написан вчера.\n\nПисатель Джек Торренс устраивается на зиму работать смотрителем роскошного отеля \"Оверлук\", расположенного вблизи снежных горных вершин. Для мужчины это – отличная возможность закончить работу над своим романом, а еще провести время с женой и сыном.\n\nНа собеседовании владелец гостиницы рассказывает главному герою о том, что предыдущий смотритель после пяти месяцев работы в отеле помешался и убил свою жену и двух дочерей. Но Джека это совершенно не пугает.\nСемейство Торренсов благополучно въезжает в отель. И здание, будто живой организм, начинает испытывать своих постояльцев на прочность.\n\nКто сможет пережить пять месяцев в снежной пустыне? И действительно ли Джек впервые оказался посетителем отеля \"Оверлук\"?",
                ImagePath = "theShining.jpg",
                AuthorId = 2
            },
            new Book
            {
                Id = 3,
                Isbn = "9785171183685",
                Name = "Преступление и наказание",
                Genre = "Роман",
                Description = "Одно из \"краеугольных\" произведений русской и мировой литературы, включенный во все школьные и университетские программы, неоднократно экранизированный роман Достоевского \"Преступление и наказание\" ставит перед читателем важнейшие нравственно-мировоззренческие вопросы – о вере, совести, грехе и об искуплении через страдание. Опровержение преступной \"идеи-страсти\", \"безобразной мечты\", завладевшей умом Родиона Раскольникова в самом \"умышленном\" и \"фантастическом\" городе на свете, составляет основное содержание этой сложнейшей, соединившей в себе несколько различных жанров книги. Задуманный как \"психологический отчет одного преступления\", роман Достоевского предстал перед читателем грандиозным художественно-философским исследованием человеческой природы, христианской трагедией о смерти и воскресении души.",
                ImagePath = "95a036bc205187af0456953a28ccccb1.jpeg",
                AuthorId = 3
            },
            new Book
            {
                Id = 4,
                Isbn = "9785171183708",
                Name = "Братья Карамазовы",
                Genre = "Роман",
                Description = "Последний, самый объёмный и один из наиболее известных романов Ф. М. Достоевского обращает читателя к вневременным нравственно-философским вопросам о грехе, воздаянии, сострадании и милосердии. Книга, которую сам писатель определил как \"роман о богохульстве и опровержении его\", явилась попыткой \"решить вопрос о человеке\", \"разгадать тайну\" человека, что, по Достоевскому, означало \"решить вопрос о Боге\". Сквозь призму истории провинциальной семьи Карамазовых автор повествует об извечной борьбе Божественного и дьявольского в человеческой душе. Один из самых глубоких в мировой литературе опытов отражения христианского сознания, \"Братья Карамазовы\" стали в ХХ веке объектом парадоксальных философских и психоаналитических интерпретаций.",
                ImagePath = "a6d50e17-c422-4c07-b73d-3b9e722fa1bb.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 5,
                Isbn = "9780743273565",
                Name = "Оно",
                Genre = "Хоррор",
                Description = "В маленьком провинциальном городке Дерри много лет назад семерым подросткам пришлось столкнуться с кромешным ужасом – живым воплощением ада. Прошли годы... Подростки повзрослели, и ничто, казалось, не предвещало новой беды. Но кошмар прошлого вернулся, неведомая сила повлекла семерых друзей назад, в новую битву со Злом. Ибо в Дерри опять льется кровь и бесследно исчезают люди. Ибо вернулось порождение ночного кошмара, настолько невероятное, что даже не имеет имени...",
                ImagePath = "i750566.jpg",
                AuthorId = 2
            },
            new Book
            {
                Id = 42,
                Isbn = "9780743273566",
                Name = "Мизери",
                Genre = "Хоррор",
                Description = "Может ли спасение от верной гибели обернуться таким кошмаром, что даже смерть покажется милосердным даром судьбы?\nМожет. Ибо это произошло с Полом Шелдоном, автором бесконечного сериала книг о злоключениях Мизери. Раненый писатель оказался в руках Энни Уилкс — женщины, потерявшей рассудок на почве его романов. Уединенный домик одержимой бесами фурии превратился в камеру пыток, а существование Пола — в ад, полный боли и ужаса.",
                ImagePath = "9785170925643--.jpg",
                AuthorId = 2
            },
            new Book
            {
                Id = 43,
                Isbn = "9785389189324",
                Name = "Наруто. Книга 1. Наруто Удзумаки",
                Genre = "Манга.Комиксы",
                Description = "Наруто Удзумаки – самый проблемный ученик академии ниндзя в деревне Коноха. День за днем он выдумывает всяческие проказы и выводит из себя окружающих! Однако даже у этого хулигана есть заветная мечта. Он собирается превзойти героев прошлого, стать величайшим ниндзя и обрести всеобщее признание! Но люди сторонятся юного Удзумаки, а кто-то даже смотрит на него с отвращением… Наруто и не подозревает, что его жизнь связана с трагедией, постигшей Коноху двенадцать лет назад…",
                ImagePath = "4394167.jpg",
                AuthorId = 4
            },
            new Book
            {
                Id = 44,
                Isbn = "9785389191358",
                Name = "Наруто. Книга 2. Мост героя!",
                Genre = "Манга.Комиксы",
                Description = "Став настоящими ниндзя, Наруто, Саскэ и Сакура получают ответственное задание – охранять знаменитого строителя мостов Тадзуну из Страны Волн. На жизнь этого старика покушаются беглый синоби Дзабудза и его подопечный Хаку, обладающий невероятными способностями. Столкновение с такими опасными противниками оборачивается трагедией, когда Саскэ закрывает собой Наруто от смертоносной атаки Хаку… Кажется, участь команды Какаси уже предрешена, но в Наруто вдруг пробуждается загадочная сила… Сможет ли он переломить ход битвы?",
                ImagePath = "4436890_1.jpg",
                AuthorId = 4
            },
            new Book
            {
                Id = 45,
                Isbn = "9785041660864",
                Name = "Я вас любил...",
                Genre = "Стихотворения",
                Description = "А.С. Пушкин (1799-1837) – величайший русский поэт, реформатор и создатель новой русской литературы, в своем творчестве придавший языку необыкновенную легкость, изысканность и одновременно точность выражения мысли; приблизивший народную речь к литературному языку, что и стало нормой. Стиль его произведений признают эталонным. Его перу было подвластно все: философская, гражданская, любовная лирика, переводы, подражания древним, сатирические жанры, в том числе эпиграммы. Свои жизненные и мировоззренческие искания Пушкин воплотил в стихотворениях, в которых отразилась широта интересов и трансформация взглядов поэта.",
                ImagePath = "9785041660864--.jpg",
                AuthorId = 1
            },
            new Book
            {
                Id = 46,
                Isbn = "9785389156487",
                Name = "Подросток",
                Genre = "Роман",
                Description = "В настоящем издании представлен роман «Подросток». Наряду с романами «Преступление и наказание», «Идиот», «Бесы», «Братья Карамазовы» он составляет так называемое великое пятикнижие Достоевского. Желание денег и власти, мечта о превосходстве над миллионами «обыкновенных», проблема отцов и детей, богоборчество – вот опасности для русского подростка. Преодолимы ли они и что победит в итоге: бунт против людей и Бога или же любовь к «живой жизни»?",
                ImagePath = "9785389156487.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 47,
                Isbn = "9785389047303",
                Name = "Идиот",
                Genre = "Роман",
                Description = "\"Главная идея... – писал Ф. М. Достоевский о своем романе \"Идиот\", – изобразить положительно-прекрасного человека. Труднее этого нет ничего на свете...\" Не для того ли писатель явил миру \"князя-Христа\", чтобы мы не забывали: \"Страдание есть главнейший и, может быть, единственный закон бытия всего человечества\".\n\nКаждое новое поколение по-своему воспринимает классику и пытается дать собственные ответы на вечные вопросы бытия. Об этом свидетельствуют и известные экранизации романа, его сценические версии. В России запоминающиеся образы князя Мышкина создали Ю. Яковлев, И. Смоктуновский, Е. Миронов.",
                ImagePath = "249816.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 48,
                Isbn = "9785389198098",
                Name = "Наруто. Книга 3. Превосходный ниндзя",
                Genre = "Манга.Комиксы",
                Description = "Наруто, Саскэ и Сакуре пришлось применить все свои силы и умения, чтобы справиться со вторым этапом экзамена на звание тюнина. Однако претендентов на участие в финальном испытании оказалось слишком много, и организаторы решили провести дополнительный этап – предварительные сражения один на один. Саскэ и Наруто удалось одолеть своих противников, бой Сакуры закончился ничьей. И теперь пришла очередь Рока Ли и Гаары: искусное тайдзюцу против непробиваемой песчаной защиты, невероятное упорство против врожденных способностей... Кто же из них победит и пройдет дальше?",
                ImagePath = "210920105901.jpeg",
                AuthorId = 4
            });
    }
}