# Windows-Служба, реализация на платформе .NET
1. Переместите папку services на диск D 
2. Далее запустите установщик от имени администратора.
3. Вы супер

Для ручной установки пропишите в командной строке windows следующее:

cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319\
installutil.exe d:\services\datawatcherservice.exe
installutil.exe -u d:\services\datawatcherservice.exe
