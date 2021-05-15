# RMLToroid Test Task
Ремейк культовой игры, выполненный в качестве тестового задания
# Описание проекта
## Base Entities
*SpriteEntity* представляет собой суперкласс для всех игровых типов в игре, предоствляя базовую функциональность, небходимую для взаимодействия с другими обьектами. Интерфейс *IBorderPackable*, реализуемый данным классом позволяет обьектам оставаться в пределах границ *Rect* камеры.
## Entities
### Asteroid
Данный класс представляет собой обьект игрового мира - астеройд, реализует интерфейс *IAsteroid*, позволяющий запустить астеройд, а также интерфейс *IPoolable*, позволящий данному обьекту быть обработанным классом *ObjectPool*, реализующим паттерн ObjectPool и Abstract Factory.
### Bullet
Данный класс представляет собой снярад игрока либо вражеского корабля, реализует интерфейс *IBullet*, позволящий выпустить снаряд, и интерфейс *IPoolable*.
### Gun
Класс оружия игрока, либо врага, реализует два метода интерфейса *IGun*, позволяющие выстрелить с орудия, и установить состояние возможности стрельбы.
### Ship
Класс корабля игрока, реализует интерфейс *IShip*, предоствляющий функциональность для взаимодесйтвия с игрой.
### UFO
Вражеский корабль, реализует интерфейсы *IUFO* и *IPoolable*.
## ObjectSpawners
Данные классы реализуют паттерн *Facade*, предоставляя простой интерфейс для спавна обьектов игры.
### GameManager
Главный класс игры, соединяющий все структуры воедино, отвечает за начальную установку состояний, и внедрении зависимостей. Стоит заметить, что данный класс не реализует паттерн *Singleton*, и таким образом не является глобально доступной сущностью, нарушая принципы SOLID.
## Global Systems
### Messaging
В качестве средства для обмена событиями между классами игры мной был выбран паттерн *BusEvent*, так как дефолтный *Observer* паттерн может привести к сильной связанности между обьектом и наблюдателем, когда событийная модель в виде Pub/Sub позволяет разьеденить связность за счет одной общий шины событий.
## ScriptableObjScripts
Классы данной категории представляют собой обьекты данных в виде *Scriptable Object* файлов, позволяющие разьединить логику обьектов и их данные, а также дающие возможность гибко настраивать пресеты для игровых обьектов гейм-дизайнерам.
# Установка
Собрано с использованием *Unity 2020.3.1f1*

