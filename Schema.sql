USE ErpSupportDB;
GO

-- 1. Şirketler Tablosu (Patronun raporlamaları için)
CREATE TABLE Companies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(200) NOT NULL,
    ContactName NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL
);

-- 2. Kullanıcılar Tablosu (Personeller ve Patron)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL, -- Güvenlik için şifreler açık metin tutulmaz
    Role NVARCHAR(20) NOT NULL, -- 'Admin' veya 'SupportStaff'
    IsActive BIT DEFAULT 1 NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL
);

-- 3. Destek Çağrıları Tablosu (Banka sırası ve SLA yönetimi için)
CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyId INT NOT NULL FOREIGN KEY REFERENCES Companies(Id),
    CreatedById INT NOT NULL FOREIGN KEY REFERENCES Users(Id), -- Çağrıyı açan
    ResolvedById INT NULL FOREIGN KEY REFERENCES Users(Id),    -- Çağrıyı çözen
    Title NVARCHAR(200) NOT NULL,
    ProblemDescription NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(30) NOT NULL,   -- 'Bekleyen', 'Cozum Asamasinda', 'Cozuldu'
    Priority NVARCHAR(20) NOT NULL, -- 'Kritik', 'Normal', 'Dusuk'
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    ResolvedAt DATETIME NULL,       -- Çözülme anında mühürlenecek (Performans için)
    UpdatedAt DATETIME NULL,        -- Sonradan değişiklik yapılırsa sadece burası güncellenir
    ResolutionText NVARCHAR(MAX) NULL -- Kılavuza itilmeden önceki ham çözüm notu
);

-- 4. Bilgi Havuzu / Kılavuz Tablosu (Arama motoru altyapısı için)
CREATE TABLE KnowledgeBase (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TicketId INT NULL FOREIGN KEY REFERENCES Tickets(Id), -- Hangi destek çağrısından doğdu?
    AuthorId INT NOT NULL FOREIGN KEY REFERENCES Users(Id), -- Kılavuza hangi personel ekledi?
    ProblemTitle NVARCHAR(255) NOT NULL,
    SolutionText NVARCHAR(MAX) NOT NULL,
    SearchKeywords NVARCHAR(500) NULL, -- Spotify tarzı hızlı tarama için anahtar kelimeler
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    ViewCount INT DEFAULT 0 NOT NULL -- En çok kullanılan çözümleri üste çıkarmak için
);
GO