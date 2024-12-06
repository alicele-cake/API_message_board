CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Id 為自動遞增，起始值為 1，每次遞增 1
    Username NVARCHAR(100) NOT NULL,    -- Username 欄位為非 NULL
    Message NVARCHAR(500) NULL,         -- Message 欄位可為 NULL
    CreatedAt DATETIME NOT NULL        -- CreatedAt 欄位為非 NULL
);
