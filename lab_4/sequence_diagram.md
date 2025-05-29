# Актор:	Администратор 
## 1.	Вход администратора
![image](https://github.com/user-attachments/assets/e47778e3-961e-413f-8bbf-26fbe919e9f1)





## Основной сценарий:
1.	Администратор отправляет логин и пароль в AuthenticationController.
2.	AuthenticationController передаёт данные в AuthenticationService.
3.	AuthenticationService передает данные на поиск учетной записи в UserRepository.
4.	AuthenticationService запрашивает пользователя по логину через UserRepository:
	UserRepository выполняет запрос к базе данных для поиска пользователя.
	Если пользователь найден:
	Проверяется правильность введённого пароля.
	При совпадении пароля — авторизация успешна, возвращается true.
5.	AuthenticationController сообщает администратору об успешной авторизации.
## Альтернативные сценарии:
•	Пользователь не найден:

После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.

AuthenticationService возвращает ошибку "пользователь не найден".

AuthenticationController уведомляет администратора об ошибке.

•	Неверный пароль:

При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.

AuthenticationController сообщает администратору о неверном пароле.

## 2.	Создание услуги
![image](https://github.com/user-attachments/assets/c6766ef1-13f6-486e-b7cc-61f12ff64ac6)





## Основной сценарий:
1.	Администратор отправляет данные о новой услуге (название, описание, правила и др.) в AdminController.
2.	AdminController передаёт данные в AdminService.
3.	AdminService сохраняет услугу через ServiceRepository.
4.	ServiceRepository делает запрос на добавление услуги в базу данных.
5.	AdminService инициирует проверку существования правил в RuleRepository.
7.	RuleRepository запрашивает наличие правил в базе данных.

## Альтернативные сценарии:
•	Правила найдены:

База данных возвращает найденные правила.

AdminService передает ServiceRepository правила для их добавления в услугу.

Услуга успешно создаётся вместе с прикреплёнными правилами.

Администратор получает сообщение об успешном создании услуги с правилами.

•	Правила не найдены:

RuleRepository сообщает об отсутствии правил.

Услуга не создается.

Администратор получает сообщение, что об ошибке выбора несуществующих правил.

## 3.	Редактирование услуги
![image](https://github.com/user-attachments/assets/62d7e98a-6814-488c-a31d-2e5c299b7bcf)




## Основной сценарий:
1.	Администратор отправляет запрос на редактирование услуги (serviceId, новое имя, новое описание, новые правила) в AdminController.
2.	AdminController вызывает метод EditService у AdminService.
3.	AdminService делает поиск по serviceId на наличие услуги.
4.	ServiceRepository создает запрос на поиск услуги, в случае если она нашлась возвращает её в AdminService.
5.	AdminService обновляет данные старой услуги через ServiceRepository
6.	ServiceRepository обращается к базе данных и обновляет дату окончания старой услуги.
7.	БД  возвращает найденную услугу в ServiceRepository
8.	ServiceRepository отправляет запрос на  создание новой услуги.
9.	ServiceRepository обновляет информацию об услуге в базе данных.
10.	После успешного обновления данных ServiceRepository сообщает об этом AdminService.
11.	AdminService инициирует обновление правил услуги через RuleRepository.

## Альтернативные сценарии:
•	Правила найдены:

База данных возвращает найденные правила.

RuleRepository обновляет правила, привязывая их к услуге.

RuleRepository подтверждает успешное прикрепление правил.

Администратор получает сообщение об успешном обновлении услуги с прикреплёнными правилами.

•	Правила не найдены:

RuleRepository сообщает об отсутствии правил.

Администратор получает сообщение, что обновляемая услуга деактивирована, но новая не создана из-за невалидных правил.

•	Услуга не найдена:

Бд возвращает пустое значение для услуги.

Значение null передается вплоть до AdminService.

AdminController передает сообщение об ошибке, что услуги не существует.




## 4.	Создание учетной записи сотрудника
![image](https://github.com/user-attachments/assets/4a373a5f-e8fe-4a66-8747-893e83255714)




## Основной сценарий:
1.	Администратор отправляет данные нового пользователя в AdminController.
2.	AdminController вызывает метод CreateStaffAccount в AdminService.
3.	AdminService проверяет, свободен ли указанный логин, обратившись к UserRepository.
4.	UserRepository делает запрос к базе данных на поиск пользователя с заданным логином.
5.	Если логин свободен: 
6. UserRepository сообщает об этом AdminService.
7. AdminService инициирует добавление нового пользователя через UserRepository. 
8. UserRepository отправляет запрос на вставку новой записи в базу данных. 
9. База данных подтверждает создание пользователя.
10. UserRepository уведомляет AdminService об успешном создании учетной записи. 
11. AdminService передаёт успешный результат в AdminController.
12. AdminController сообщает администратору об успешном создании учетной записи.

## Альтернативный сценарий:

•	Логин занят:

UserRepository сообщает AdminService, что логин уже используется.

AdminService отправляет в AdminController сообщение об ошибке.

AdminController уведомляет администратора об ошибке: логин уже занят.

# Актор:	Госслужащий.
## 5.	Вход госслужащего в систему
![image](https://github.com/user-attachments/assets/55f22d82-f38f-4bac-b04b-f023e3212571)




## Основной сценарий:
1.	Госслужащий отправляет логин и пароль в AuthenticationController.
2.	AuthenticationController передаёт данные в AuthenticationService.
3.	AuthenticationService передает данные на поиск учетной записи в UserRepository.
4.	AuthenticationService запрашивает пользователя по логину через UserRepository:
	UserRepository выполняет запрос к базе данных для поиска пользователя.
	Если пользователь найден:
	Проверяется правильность введённого пароля.
	При совпадении пароля — авторизация успешна, возвращается true.
5.	AuthenticationController сообщает администратору об успешной авторизации.
## Альтернативные сценарии:
•	Пользователь не найден:

После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.

AuthenticationService возвращает ошибку "пользователь не найден".

AuthenticationController уведомляет администратора об ошибке.

•	Неверный пароль:

При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.

AuthenticationController сообщает госслужащему о неверном пароле.

## 6.	Добавление результата по заявке
![image](https://github.com/user-attachments/assets/ee95ec32-9dee-455d-8af5-870feb6f4b2c)



## Основной сценарий:
1.	Запрос на обработку заявки:
Госслужащий  отправляет запрос на обработку заявки через CivilServantController с идентификатором заявки (applicationId) и результатом (result) через метод AddResult (applicationId, result).
2.	Обработка заявки в сервисе:
CivilServantController передает запрос в CivilServantService для обработки заявки, вызвав метод AddResult(applicationId, result).
3.	Поиск заявки в базе данных:
В CivilServantService происходит запрос в репозиторий ApplicationRepository с целью получить заявку по идентификатору через метод findbyId (applicationId).
В ApplicationRepository выполняется запрос в базу данных: SELECT * FROM Applications WHERE id = applicationId для поиска заявки.
4.	Проверка наличия заявки:
Если заявка найдена, данные заявки передаются обратно в CivilServantService.
Если заявка не найдена, система возвращает ошибку в CivilServantController, уведомляя госслужащего о том, что заявка не существует.
5.	Обновление результата заявки:
Если заявка найдена, происходит обновление результата заявки. В CivilServantService выполняется запрос на обновление данных заявки в репозитории: UPDATE Applications SET result = result WHERE id = applicationId.
После успешного обновления, CivilServantService возвращает подтверждение о том, что заявка была успешно обновлена.
6.	Ответ госслужащему:
CivilServantController получает результат из CivilServantService и отправляет уведомление госслужащему о том, что заявка была успешно обновлена.
7.	Ошибка при отсутствии заявки:
Если заявка не найдена в базе данных, возвращается сообщение об ошибке, и госслужащий получает уведомление о том, что заявка не существует.

## 7.	Изменение статуса заявки
![image](https://github.com/user-attachments/assets/ec4f96d3-3d02-4196-8e10-8509af83ed61)



## Основной сценарий:
1.	Запрос на изменение статуса:
Госслужащий (актер civilServant) отправляет запрос через CivilServantController для изменения статуса заявки. Запрос включает идентификатор заявки (applicationId) и новый статус (status) через метод UpdateStatus(applicationId, status).
2.	Обработка запроса в сервисе:
CivilServantController передает запрос в CivilServantService, вызвав метод UpdateStatus(applicationId, status).
3.	Поиск заявки в базе данных:
В CivilServantService выполняется запрос в репозиторий ApplicationRepository, чтобы получить заявку по идентификатору через метод findbyId(applicationId).
В ApplicationRepository выполняется запрос в базу данных: SELECT * FROM Applications WHERE id = applicationId для получения данных заявки.
4.	Проверка наличия заявки:
Если заявка найдена, данные заявки передаются обратно в CivilServantService.
Если заявка не найдена, система возвращает ошибку в CivilServantController, уведомляя госслужащего о том, что заявка не существует.
5.	Валидация нового статуса:
В CivilServantService выполняется валидация нового статуса заявки. Если статус является допустимым, процесс продолжается. В случае недопустимого статуса, система возвращает ошибку о недопустимом статусе.
6.	Обновление статуса заявки:
Если статус допустим, в CivilServantService выполняется запрос на обновление статуса заявки в базе данных: UPDATE Applications SET status = status WHERE id = applicationId.
После успешного обновления, система возвращает сообщение о том, что статус был успешно изменен.
7.	Ответ госслужащему:
CivilServantController получает результат из CivilServantService и отправляет уведомление госслужащему о том, что статус заявки был успешно обновлен.
8.	Ошибка при недопустимом статусе или отсутствии заявки:
Если статус заявки недопустим или заявка не найдена, возвращаются ошибки, и госслужащий получает уведомление об ошибке.

## 8.	Обработка заявки на регистрацию
![image](https://github.com/user-attachments/assets/0e8140f1-4e4b-44b1-86a7-30a4172861b3)




## Основной сценарий:
1.	Сотрудник проверяет заявку через систему:
Вызывает метод ProcessRegistrationRequest(applicationId, approved) 
2.	Система ищет заявку:
findbyId(applicationId) → Возвращает объект application.
3.	Проверка статуса:
Check Approved (approved) → Подтверждает одобрение.
4.	Обновление статуса в БД:
UpdateStatus(applicationId, applicationStatus) → Меняет статус заявления→ для БД: UPDATE Applications SET status = 'REJECTED' WHERE id = applicationId;
5.	Результат:
return Вы одобрили заявку на регистрацию → Уведомление об успешном одобрении.

## Альтернативные сценарии:
A. Заявка не одобрена:
1.	Система возвращает:
Return Вы не одобрили заявку на регистрацию → Статус "ОТКАЗ".
2.	Обновление в БД:
UPDATE Applications SET status = 'REJECTED' WHERE id = applicationId;

B. Заявка не найдена:
1.	Return Ошибка: Заявление не найдено → Возвращает null.
2.	SQL-запрос не находит данных: return null

# Актор: Пользователь.

## 9.	Вход пользователя
![image](https://github.com/user-attachments/assets/73098505-5328-4d1e-9254-37e65d9c023e)



## Основной сценарий:
1.	Пользователь вводит логин/пароль → отправляет в AuthenticationController login(login, password). Контроллер принимает данные и передает их в AuthenticationService
2.	AuthenticationService → вызывает UserRepository через findByLogin(login)
Репозиторий ищет пользователя в базе данных
Выполняется SQL: SELECT * FROM users WHERE login = login
3.	При успешном нахождении пользователя:
AuthenticationService → проводит проверку пароля CheckPassword(password)
4.	Если пароль верный:
AuthenticationService → Фиксирует успешную попытку входа и возвращает значение true(как успешный вход)
5.	AuthenticationController получает ответ и передает сообщение пользователю.

## Альтернативные сценарии:

Сценарий 1: Неверный пароль

1.	После проверки в CheckPassword(password):
Возвращается false
AuthenticationService передает значение false как ошибку входа
2.	AuthenticationController передает сообщение "Неверный пароль"

Сценарий 2: Пользователь не найден

1.	UserRepository не находит запись → возвращает null
2.	AuthenticationService возвращает false значение с пометкой о не найденом пользователе.
3.	AuthenticationController → возвращает сообщение "Пользователь не найден"

## 10.	Заявка на регистрацию аккаунта
![image](https://github.com/user-attachments/assets/569b4ff6-81b7-459d-838c-0af61373045a)



## Основной сценарий:
1.	Пользователь вызывает RegisterUser(login, password, fullName) в AuthenticationController
2.	AuthenticationController передает данные через RegisterUser(login, password, fullName) в AuthenticationService
3.	AuthenticationService выполняет:
findbyEmail(login) - проверка существования пользователя
SQL: SELECT * FROM User WHERE login = login
Получает return null (пользователь не найден)
4.	AuthenticationService создает пользователя:
save(user) - сохранение нового пользователя
SQL: INSERT INTO User (login, password, fullName) VALUES (...)
Получает return user (созданный пользователь)
5.	Возврат цепочки:
AuthenticationService → AuthenticationController: return user
AuthenticationController → Пользователь: "Вы успешно подали заявку на регистрацию"

## Альтернативный сценарий (пользователь существует):
1.	При проверке findbyEmail(login):
SQL-запрос находит существующую запись
Возвращается return user (существующий пользователь)
2.	AuthenticationService прерывает процесс:
Возвращает ошибку: "Пользователь зарегистрирован, попробуйте зайти или другой логин"
3.	AuthenticationController показывает сообщение об ошибке пользователю

## 11.	Подать заявление на оказание услуги
![image](https://github.com/user-attachments/assets/682b4c72-4d67-4e5e-b346-81111ed66fe9)



## Основной сценарий (успешное создание заявки):
1.	Пользователь вызывает метод GetAllActivesServices()для начала процесса
Система возвращает return Service (список доступных услуг)
2.	Система выполняет поиск услуг:
SQL: SELECT * FROM Services
Возвращает список return Services
3.	Пользователь выбирает услугу через SubmitApplication (application)
4.	Система создает заявку:
SQL: INSERT INTO Applications (...) VALUES (...)
Возвращает данные заявки (return applicationData)

## Альтернативный сценарий (услуга не найдена):
1.	При попытке выбора услуги система возвращает:
"Нужная услуга не была найдена"

## 12.	Отмена заявления на услугу
![image](https://github.com/user-attachments/assets/5c651ec8-a9cd-4fbc-8042-9a39f114d164)




## Основной сценарий:
1.	Пользователь инициирует запрос на отмену заявки с помощью метода CancelApplication(applicationId) в UserController.
2.	UserController вызывает метод CancelApplication(applicationId) в UserService.
3.	В UserService происходит запрос в ApplicationRepository для поиска заявки по ID (findbyId(applicationId)).
4.	ApplicationRepository делает SQL-запрос в базу данных для получения данных о заявке:
SELECT * FROM Applications WHERE id = applicationId
5.	Если заявка найдена:
В UserService обновляется статус заявки на "CANCELED" и этот статус сохраняется в базе данных с помощью запроса UPDATE Applications SET status = 'CANCELED' WHERE id = applicationId.
База данных подтверждает успешное обновление.
6.	Если заявка не найдена, UserService отправляет ошибку обратно в UserController.
7.	UserController возвращает пользователю сообщение о том, что заявка отменена, либо что заявка не найдена.

## 13.	Узнать статус заявления
![image](https://github.com/user-attachments/assets/edf29aad-18bd-4854-9aff-62553324bffc)



## Основной сценарий:
1.	User получает статус заявки, передавая ApplicationController идентификатор заявки.
2.	ApplicationController вызывает GetStatus из ApplicationService, который в свою очередь вызывает findbyId из ApplicationRepository, и далее возвращается результат запроса к БД.
3.	БД возвращает, найденную заявку заявку.
4.	ApplicationService возвращается её статус.
5.	ApplicationController выводит статус заявки
## Альтернативный сценарий:
Заявка не существует :

БД возвращает пустое значение.

ApplicationService возвращает ошибку.

ApplicationController возвращает сообщение об ошибке, что заявка не найдена.
