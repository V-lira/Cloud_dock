CREATE TABLE IF NOT EXISTS tickets (
    id SERIAL PRIMARY KEY,
    title VARCHAR(120) NOT NULL,
    description TEXT NOT NULL,
    priority INT CHECK (priority BETWEEN 1 AND 3),
    status VARCHAR(20) DEFAULT 'new' CHECK (status IN ('new', 'in_progress', 'done')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

--тестовые заявочки:
INSERT INTO tickets (title, description, priority, status) VALUES 
('Не работает принтер', 'Принтер в офисе 204 не печатает документы.', 2, 'new'),
('Забыл пароль', 'Нужен сброс пароля от корпоративной почты.', 1, 'in_progress'),
('Обновить Windows', 'На всех ноутбуках отдела разработки нужно обновить ОС.', 3, 'new'),
('Сломался стул', 'Кресло скрипит и не держит спинку.', 1, 'done'),
('Нет доступа к Git', 'Не могу запушить изменения в репозиторий.', 2, 'new');