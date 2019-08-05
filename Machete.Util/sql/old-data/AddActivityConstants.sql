IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityType' AND [key]='Organizing Meeting')
BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated)
VALUES ('activityType', 'Organizing Meeting', 'Organizing Meeting', 'Reunión de Organizadores', 0, 0, GETDATE(), GETDATE()) END

IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityName' AND [key]='Organizing Meeting')
BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated)
VALUES ('activityName', 'Organizing Meeting', 'Organizing Meeting', 'Reunión de Organizadores', 0, 0, GETDATE(), GETDATE()) END

IF NOT EXISTS (SELECT 1 FROM dbo.Lookups WHERE category='activityName' AND [key]='Assembly')
BEGIN INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated)
VALUES ('activityName', 'Assembly', 'Assembly', 'Asamblea', 0, 0, GETDATE(), GETDATE()) END

IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Class' AND category = 'activityType')
BEGIN UPDATE dbo.Lookups SET [key] = 'Class'
WHERE text_EN = 'Class' AND category = 'activityType' END
ELSE INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated) 
VALUES ('activityType', 'Class', 'Class', 'Clase', 0, 0, GETDATE(), GETDATE())

IF EXISTS (SELECT 1 FROM dbo.Lookups WHERE text_EN = 'Assembly' AND category = 'activityType')
BEGIN UPDATE dbo.Lookups SET [key] = 'Assembly'
WHERE text_EN = 'Assembly' AND category = 'activityType' END
ELSE INSERT INTO dbo.Lookups (category, [key], text_EN, text_ES, speciality, selected, datecreated, dateupdated)
VALUES ('activityType', 'Assembly', 'Assembly', 'Asamblea', 0, 0, GETDATE(), GETDATE())
