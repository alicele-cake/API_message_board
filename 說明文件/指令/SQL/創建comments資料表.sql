CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Id ���۰ʻ��W�A�_�l�Ȭ� 1�A�C�����W 1
    Username NVARCHAR(100) NOT NULL,    -- Username ��쬰�D NULL
    Message NVARCHAR(500) NULL,         -- Message ���i�� NULL
    CreatedAt DATETIME NOT NULL        -- CreatedAt ��쬰�D NULL
);
