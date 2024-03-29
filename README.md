# ZigZag

Тестовое задание для Unity разработчика - ZigZag

Описание:
В игре, которую предстоит реализовать есть 3 игровых сущности - шарик, которым управляет игрок, поле, по которому двигается шарик, и кристаллы, расположенные на поле. 

Поле представляет собой набор квадратных тайлов, расположенных рядом друг с другом, по которым может катиться шарик. Если шарик выходит за рамки тайла в пустое пространство (туда, где нет тайла), то  шарик падает вниз за пределы поля, и игрок проигрывает. В случае проигрыша игра начинается заново по клику в любой точке экрана. 

Поле представляет собой дорожку толщиной в n тайлов, направление которой может идти только прямо или направо. Поле генерируется случайно до бесконечности, таким образом, чтобы по нему мог пройти шарик (не должно быть непроходимых участков или тупиков). В самом начале игры поле всегда представляет собой квадратную площадку тайлов размером 3*3, генерация дорожки шириной в 1 тайл начинается только после этой площадки.

На каждом тайле может быть 1 кристалл или ни одного. Условно поделим тайлы на блоки по 5 тайлов в каждом. На каждом из таких блоков должен быть появиться 1 кристалл. На каком именно тайле из блока появится кристалл определяется по одному из 2 правил:
случайным образом
по порядку. То есть на первом блоке - 1-ый тайл с кристаллом, на 2-ом - 2 тайл и так далее до 5-ого блока. Далее опять с 1-ого по 5.
Какое из этих двух правил в игре будет работать должно конфигурироваться некоторым параметром.

Если шарик наезжает на кристалл, то кристалл исчезает с тайла и считается, что игрок подобрал этот кристалл.

Шарик постоянно движется по полю с одинаковой скоростью. Скорость шарика должна конфигурироваться некоторым параметром. Шарик может двигаться либо прямо, либо вправо. Изменение направления движения шарика происходит по клику в любую часть игрового поля. Если шарик двигается прямо, то после клика, он начнет двигаться вправо. После следующего клика шарик снова начнет двигаться прямо. 

Камера двигается за шариком таким образом, чтобы он всегда находился в центре экрана по вертикали. Когда игрок начинает игру, шарик стоит на месте и начинает движение только после клика в любую часть экрана. Радиус шарика должен быть приблизительно ½ от размера тайла по любой из осей (тайл либо квадрат, либо куб). 

Последним пунктом реализуем понятие сложности уровня. Возьмем следующее правило:
В простом уровне сложности толщина дорожки тайлов (n)  равна 3 тайлам
В среднем уровне сложности толщина дорожки тайлов (n) равна 2
В сложном n = 1
Уровень сложности игры должен конфигурироваться некоторым параметром.

Дополнительные задания:
Реализуйте игру с использованием Zenject.
При проигрыше уровень должен начинаться заново без перезапуска сцены.
Реализуйте систему подсчета очков и выводите результат на экран, при условии, что за подбор одного кристалла начисляется 1 очко. Для реализации этого пункта можно использовать любой UI инструмент на ваш выбор (NGUI, Unity UI, IMGUI,  или просто сделать на обычных Gameobject).
Добавьте анимации пропадания тайлов, находящихся позади шарика (те, которые уже были пройдены). Они могут падать вниз за границы экрана, либо уходить в прозрачность.

Референс:
В качестве примера основных механик следует посмотреть на игру:
на андроид: https://play.google.com/store/apps/details?id=com.ketchapp.zigzaggame&hl=ru
на iOS: https://itunes.apple.com/ru/app/zigzag/id951364656?mt=8
или игровое видео на youtube: https://youtu.be/9Uqux7wuP0M?t=45s
Важно заметить, что эта игра не реализует некоторые пункты, описанные в этом задании. 
Что учитывается при оценке:
Грамотно построенная архитектура с заделом на расширяемость игры. Под расширяемостью имеется в виду возможность потенциального сопровождения вашего решения, возможность внесения изменений в механики с минимальными изменениями существующего кода.
Чистый, читаемый код.
Демонстрация знания паттернов и принципов ООП.
Выполненное задание запускается без ошибок.

Что не учитывается при оценке:
Визуальная составляющая выполненного задания. Вы можете подыскать красивые модели/спрайты для задания и сделать красивые анимации, но можете обойтись и примитивами, которые встроены в саму Unity3D (Sphere, Cube, Capsule) .
Кроме того, не обязательно должен повторяться визуальный стиль референсной игры. Вы можете сделать задание как в 2D, так и в 3D. Также необязательно использовать физику в решении - перемещение шара и выход за границы могут быть реализованы посредством несложных математических вычислений.
Наличие звукового сопровождения и музыки.
Верстка и графическое оформление UI. Достаточно надписей а ля “Вы проиграли” или “Tap to start”. 
Наличие сборок под Android или Windows/Mac. Достаточно того, чтобы игра работала в редакторе Unity3D.
Формат сдачи тестового задания:
В качестве выполненного тестового задания, предоставьте ссылку на git репозиторий, куда вы загрузили свое решение. Можете использовать любой любой сервис на ваш выбор - https://github.com/ https://bitbucket.org/product/ или любой другой.

Рекомендации:
Не используйте реактивный подход при выполнении этого задания (например, UniRx)
Если у вас нет знаний/опыта работы с Внедрением Зависимостей (DI), рекомендуется не выполнять дополнительный пункт #1 (Реализуйте игру с использованием Zenject). DI является достаточно сложным подходом, грамотное изучение которого невозможно в рамках данного тестового задания. Если же вы имеете опыт работы с DI, но не имеете опыта работы конкретно с Zenject, можете использовать любой другой IoC контейнер при выполнении этого пункта. Подход “Внедрение для бедных” (без использования какого-либо IoC контейнера) также разрешен к применению. В случае, если вы реализуете это задание на другом IoC контейнере или без него, выполнение дополнительного пункта будет засчитано.