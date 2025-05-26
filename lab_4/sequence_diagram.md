# Актор:	Администратор 
## 1.	Вход администратора
![image](https://github.com/user-attachments/assets/437dc5e0-ef59-465a-9535-f72405b38ef1)

## Основной сценарий:
1.	Администратор отправляет логин и пароль в AuthenticationController.
2.	AuthenticationController передаёт данные в AuthenticationService.
3.	AuthenticationService передает данные на поиск учетной записи в UserRepository.
4.	AuthenticationService запрашивает пользователя по логину через UserRepository:
	UserRepository выполняет запрос к базе данных для поиска пользователя.
	Если пользователь найден:
	Проверяется правильность введённого пароля.
	При совпадении пароля — авторизация успешна, возвращается User.
5.	AuthenticationController сообщает администратору об успешной авторизации.
## Альтернативные сценарии:
•	Пользователь не найден:
o	После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.
o	AuthenticationService возвращает ошибку "пользователь не найден".
o	AuthenticationController уведомляет администратора об ошибке.
•	Неверный пароль:
o	При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.
o	AuthenticationController сообщает администратору о неверном пароле.
