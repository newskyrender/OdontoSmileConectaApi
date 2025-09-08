-- Script SQL para adicionar colunas created_at e updated_at nas tabelas relacionais
-- que já existem mas não possuem essas colunas

-- Adicionar colunas na tabela profissional_especialidades
ALTER TABLE profissional_especialidades 
ADD COLUMN created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
ADD COLUMN updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6);

-- Adicionar colunas na tabela profissional_equipamentos  
ALTER TABLE profissional_equipamentos
ADD COLUMN created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
ADD COLUMN updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6);

-- Adicionar colunas na tabela profissional_facilidades
ALTER TABLE profissional_facilidades 
ADD COLUMN created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
ADD COLUMN updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6);
