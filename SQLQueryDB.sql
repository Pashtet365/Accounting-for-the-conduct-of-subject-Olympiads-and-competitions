-- �������� ������� "�����������"
CREATE TABLE ����������� (
  id_����������� INT PRIMARY KEY IDENTITY,
  ������������ NVARCHAR(255) NOT NULL,
  ����� NVARCHAR(255) NOT NULL,
  ����� NVARCHAR(255) NOT NULL
);

-- �������� ������� "��������"
CREATE TABLE �������� (
  id_�������� INT PRIMARY KEY IDENTITY,
  ��� NVARCHAR(255) NOT NULL,
  ��������� NVARCHAR(255) NOT NULL,
  id_����������� INT FOREIGN KEY REFERENCES �����������(id_�����������)
);

-- �������� ������� "���������"
CREATE TABLE ��������� (
  id_��������� INT PRIMARY KEY IDENTITY,
  ��� NVARCHAR(255) NOT NULL,
  ����� NVARCHAR(50) NULL,
  id_����������� INT FOREIGN KEY REFERENCES �����������(id_�����������)
);

-- �������� ������� "��������"
CREATE TABLE �������� (
  id_�������� INT PRIMARY KEY IDENTITY,
  ������������ NVARCHAR(255) NOT NULL
);

-- �������� ������� "�����"
CREATE TABLE ����� (
  id_����� INT PRIMARY KEY IDENTITY,
  ������������ NVARCHAR(255) NOT NULL
);

-- �������� ������� "�����������"
CREATE TABLE ����������� (
  id_����������� INT PRIMARY KEY IDENTITY,
  �������� NVARCHAR(255) NOT NULL,
  id_�������� INT FOREIGN KEY REFERENCES ��������(id_��������),
  ����_���������� DATE NOT NULL,
  id_����� INT FOREIGN KEY REFERENCES �����(id_�����) NOT NULL
);

-- �������� ������� "������"
CREATE TABLE ������ (
  id_������ INT PRIMARY KEY IDENTITY,
  id_�������� INT FOREIGN KEY REFERENCES ��������(id_��������),
  id_����������� INT FOREIGN KEY REFERENCES �����������(id_�����������) NOT NULL,
  id_����������� INT FOREIGN KEY REFERENCES �����������(id_�����������),
  ����_������ DATE NOT NULL
);

-- �������� ������� "�����������_������"
CREATE TABLE �����������_������ (
  id_����������� INT PRIMARY KEY IDENTITY,
  id_��������� INT FOREIGN KEY REFERENCES ���������(id_���������),
  id_����� INT FOREIGN KEY REFERENCES �����(id_�����),
  ����_����������� DATE NOT NULL,
  ����� FLOAT NOT NULL,
  ������ NVARCHAR(50) NOT NULL,
  id_������ INT FOREIGN KEY REFERENCES ������(id_������) NULL
);


CREATE TABLE [dbo].[������] (
    [id_������]    INT IDENTITY (1, 1) NOT NULL,
    [id_���������] INT NOT NULL,
    [id_������]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([id_������] ASC),
    FOREIGN KEY ([id_���������]) REFERENCES [dbo].[���������] ([id_���������]),
    FOREIGN KEY ([id_������]) REFERENCES [dbo].[������] ([id_������])
);