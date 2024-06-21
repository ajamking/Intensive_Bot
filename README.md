Проект - телеграм бот предназначенный для 

Разработан с использованием языка C# и платформы .NET 6.0
Настройка / Запуск:
C:\Users\tedto\.nuget\packages\microsoft.

# Описание
Бот на стадии MVP. Основное предназначение -  получение информации об активных MergeRequest-ах c GitLab-a пользователя.

Код написан на языке C# для платформы .NET 6.0. 

# Инструкция
1. Скачать репозиторий;
2. Желательно открыть его в IDE (например VisualStudio), проверить все ли зависимости и nuget-ы подгрузились (полный перечень ниже). Если не подгрузились - установить;
3. Заполнить своими данными файл Intensive_Bot\Intensive_Bot\EnvironmentFiles\Environment.json (пример ниже);
4. Запустить программу;
5. Зайти в телеграм и начать чат с ботом - https://t.me/TACh_Intensive_Bot;
6. Вы также можете создать своего бота, использовать его токен и общаться с ним.
7. Если все данные в файле Environment.json были указаны верно, вы получите доступ к функционалу бота.

# Список nuget-ов
```
extensions.hosting\8.0.0\
newtonsoft.json\13.0.3\
serilog\4.0.0\
serilog.sinks.console\6.0.0\
serilog.sinks.file\5.0.0\
telegram.bot\19.0.0\
```

# Ожидаемое содержание файла Environment.json

```
{
  "BotToken": "", - Укажите токен своего бота
  "AdminUsername": "", - Укажите cвой username в telegram
  "LogFilePath": "./EnvironmentFiles/LogFile.txt",  - Можно оставить как есть
  "ExceptionLogFilePath": "./EnvironmentFiles/ExceptionLogFile.txt", - Можно оставить как есть
  "GitLabAuthToken": "glpat-RiERFtxJzNdzcWSp9xyS", - Укажите свой гитлаб токен. Если нет - создайте https://gitlab.com/-/user_settings/personal_access_tokens
  "GitLabUrl": "https://gitlab.com/api/v4" #Этот параметр нужен для обращения к актуальной версии gitlab api, изменять его не нужно.
}
```
