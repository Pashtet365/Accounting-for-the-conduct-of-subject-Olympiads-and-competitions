-- Создание таблицы "Организация"
CREATE TABLE Организация (
  id_организации INT PRIMARY KEY IDENTITY,
  наименование NVARCHAR(255) NOT NULL,
  район NVARCHAR(255) NOT NULL,
  город NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Педагоги"
CREATE TABLE Педагоги (
  id_педагога INT PRIMARY KEY IDENTITY,
  фио NVARCHAR(255) NOT NULL,
  должность NVARCHAR(255) NOT NULL,
  id_организации INT FOREIGN KEY REFERENCES Организация(id_организации)
);

-- Создание таблицы "Участники"
CREATE TABLE Участники (
  id_участника INT PRIMARY KEY IDENTITY,
  фио NVARCHAR(255) NOT NULL,
  класс NVARCHAR(50) NULL,
  id_организации INT FOREIGN KEY REFERENCES Организация(id_организации)
);

-- Создание таблицы "Предметы"
CREATE TABLE Предметы (
  id_предмета INT PRIMARY KEY IDENTITY,
  наименование NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Этапы"
CREATE TABLE Этапы (
  id_этапа INT PRIMARY KEY IDENTITY,
  наименование NVARCHAR(255) NOT NULL
);

-- Создание таблицы "Мероприятия"
CREATE TABLE Мероприятия (
  id_мероприятия INT PRIMARY KEY IDENTITY,
  название NVARCHAR(255) NOT NULL,
  id_предмета INT FOREIGN KEY REFERENCES Предметы(id_предмета),
  дата_проведения DATE NOT NULL,
  id_этапа INT FOREIGN KEY REFERENCES Этапы(id_этапа) NOT NULL
);

-- Создание таблицы "Заявка"
CREATE TABLE Заявка (
  id_заявки INT PRIMARY KEY IDENTITY,
  id_педагога INT FOREIGN KEY REFERENCES Педагоги(id_педагога),
  id_организации INT FOREIGN KEY REFERENCES Организация(id_организации) NOT NULL,
  id_мероприятия INT FOREIGN KEY REFERENCES Мероприятия(id_мероприятия),
  дата_подачи DATE NOT NULL
);

-- Создание таблицы "Прохождение_этапов"
CREATE TABLE Прохождение_этапов (
  id_прохождения INT PRIMARY KEY IDENTITY,
  id_участника INT FOREIGN KEY REFERENCES Участники(id_участника),
  id_этапа INT FOREIGN KEY REFERENCES Этапы(id_этапа),
  дата_прохождения DATE NOT NULL,
  баллы FLOAT NOT NULL,
  статус NVARCHAR(50) NOT NULL,
  id_заявки INT FOREIGN KEY REFERENCES Заявка(id_заявки) NULL
);


CREATE TABLE [dbo].[Список] (
    [id_списка]    INT IDENTITY (1, 1) NOT NULL,
    [id_участника] INT NOT NULL,
    [id_заявки]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([id_списка] ASC),
    FOREIGN KEY ([id_участника]) REFERENCES [dbo].[Участники] ([id_участника]),
    FOREIGN KEY ([id_заявки]) REFERENCES [dbo].[Заявка] ([id_заявки])
);