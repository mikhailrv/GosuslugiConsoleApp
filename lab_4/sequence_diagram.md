# Актор:	Администратор 
## 1.	Вход администратора
![image](https://github.com/user-attachments/assets/516fffdf-c8da-4862-80a9-b598a3e4b7e8)








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

1. После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.

2. AuthenticationService возвращает ошибку "пользователь не найден".

3. AuthenticationController уведомляет администратора об ошибке.

•	Неверный пароль:

1. При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.

2. AuthenticationController сообщает администратору о неверном пароле.

## 2.	Создание услуги
![image](https://github.com/user-attachments/assets/13b3c049-f30e-4094-adc5-b2c2d3953f65)






## Основной сценарий:
1.	Администратор отправляет данные о новой услуге (название, описание, правила и др.) в AdminController.
2.	AdminController передаёт данные в AdminService.
3.	AdminService сохраняет услугу через ServiceRepository.
4.	ServiceRepository делает запрос на добавление услуги в базу данных.
5.	AdminService инициирует проверку существования правил в RuleRepository.
7.	RuleRepository запрашивает наличие правил в базе данных.

## Альтернативные сценарии:
•	Правила найдены:

1. База данных возвращает найденные правила.

2. AdminService передает ServiceRepository правила для их добавления в услугу.

3. Услуга успешно создаётся вместе с прикреплёнными правилами.

4. Администратор получает сообщение об успешном создании услуги с правилами.

•	Правила не найдены:

1. RuleRepository сообщает об отсутствии правил.

2. Услуга создается без правил.

3. Администратор получает сообщение, что услуга создана без правил.

## 3.	Редактирование услуги
![image](https://github.com/user-attachments/assets/267e56b7-e874-4e55-b205-e8ef6b68025f)










## Основной сценарий:
1.	Администратор отправляет запрос на редактирование услуги в AdminController.
2.	AdminController вызывает метод EditService у AdminService.
3.	AdminService делает поиск по serviceId на наличие услуги.
4.	ServiceRepository создает запрос на поиск услуги, в случае если она нашлась возвращает её в AdminService.
5.	AdminService обновляет данные старой услуги через ServiceRepository
6.	ServiceRepository обращается к базе данных и обновляет дату окончания старой услуги.
7.	БД  возвращает найденную услугу в ServiceRepository
8.	ServiceRepository отправляет запрос на  создание новой услуги.
9.	После успешного создания услуги ServiceRepository передает AdminService id новой услуги.
10.	AdminService инициирует обновление правил услуги через RuleRepository.

## Альтернативные сценарии:
•	Правила найдены:

1. База данных возвращает найденные правила.

2. RuleRepository обновляет правила, привязывая их к услуге.

3. RuleRepository подтверждает успешное прикрепление правил.

4. Администратор получает сообщение об успешном обновлении услуги с прикреплёнными правилами.

•	Правила не найдены:

1. RuleRepository сообщает об отсутствии правил.

2. Администратор получает сообщение, что обновляемая услуга деактивирована, но новая не создана из-за невалидных правил.

•	Услуга не найдена:

1. Бд возвращает пустое значение для услуги.

2. Значение null передается вплоть до AdminService.

3. AdminController передает сообщение об ошибке, что услуги не существует.




## 4.	Создание учетной записи сотрудника
![image](https://github.com/user-attachments/assets/ae144ecb-0e2b-4b4d-9089-a258db0ad46a)





## Основной сценарий:
1.	Администратор отправляет данные нового пользователя в AdminController.
2.	AdminController вызывает метод CreateStaffAccount в AdminService.
3.	AdminService проверяет, свободен ли указанный логин, обратившись к UserRepository.
4.	UserRepository делает запрос к базе данных на поиск пользователя с заданным логином.
5.	Если логин свободен: UserRepository сообщает об этом AdminService.
6. AdminService инициирует добавление нового пользователя через UserRepository. 
7. UserRepository отправляет запрос на вставку новой записи в базу данных. 
8. База данных подтверждает создание пользователя.
9. UserRepository уведомляет AdminService об успешном создании учетной записи. 
10. AdminService передаёт успешный результат в AdminController.
11. AdminController сообщает администратору об успешном создании учетной записи.

## Альтернативный сценарий:

•	Логин занят:

1. UserRepository сообщает AdminService, что логин уже используется.

2. AdminService отправляет в AdminController сообщение об ошибке.

3. AdminController уведомляет администратора об ошибке: логин уже занят.

# Актор:	Госслужащий.
## 5.	Вход госслужащего в систему
![image](https://github.com/user-attachments/assets/2279d4b2-96cd-4fcd-9b0d-35273bd2eee9)





## Основной сценарий:
1.	Госслужащий отправляет логин и пароль в AuthenticationController.
2.	AuthenticationController передаёт данные в AuthenticationService.
3.	AuthenticationService передает данные на поиск учетной записи в UserRepository.
4.	AuthenticationService запрашивает пользователя по логину через UserRepository:
	UserRepository выполняет запрос к базе данных для поиска пользователя.
	Если пользователь найден:
	Проверяется правильность введённого пароля.
	При совпадении пароля — авторизация успешна, возвращается true.
5.	AuthenticationController сообщает госслужащему об успешной авторизации.
## Альтернативные сценарии:
•	Пользователь не найден:

1. После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.

2. AuthenticationService возвращает ошибку "пользователь не найден".

3. AuthenticationController уведомляет госслужащего об ошибке.

•	Неверный пароль:

1. При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.

2. AuthenticationController сообщает госслужащему о неверном пароле.

## 6.	Добавление результата по заявке
![image](https://github.com/user-attachments/assets/36be84e8-9869-4585-968e-efe4c9fe2a6c)






## Основной сценарий:
1.	Запрос на обработку заявки:
Госслужащий  отправляет запрос на обработку заявки через CivilServantController с идентификатором заявки (applicationId) и результатом (result) через метод AddResult (applicationId, result).
2.	Обработка заявки в сервисе:
CivilServantController передает запрос в CivilServantService для обработки заявки, вызвав метод AddResultToApplication(applicationId, result).
3.	Поиск заявки в базе данных:
В CivilServantService происходит запрос в репозиторий ApplicationRepository с целью получить заявку по идентификатору через метод FindById (applicationId).
В ApplicationRepository выполняется запрос в базу данных: SELECT * FROM Applications WHERE id = applicationId для поиска заявки.
4.	Проверка наличия заявки:
Если заявка найдена, данные заявки передаются обратно в CivilServantService.
Если заявка не найдена, система возвращает ошибку в CivilServantController, уведомляя госслужащего о том, что заявка не существует.
5.	Обновление результата заявки:
Если заявка найдена, происходит обновление результата заявки. CivilServantService вызывает метод репозитория, который обращается к БД: UPDATE Applications SET result = result WHERE id = applicationId.
После успешного обновления, CivilServantService возвращает подтверждение о том, что заявка была успешно обновлена.
6.	Ответ госслужащему:
CivilServantController получает результат из CivilServantService и уведомляет госслужащего о том, что заявка была успешно обновлена.
7.	Ошибка при отсутствии заявки:
Если заявка не найдена в базе данных, возвращается сообщение об ошибке, и госслужащий получает уведомление о том, что заявка не найдена.

## 7.	Изменение статуса заявки
![image](https://github.com/user-attachments/assets/d10ba5a5-f11c-459c-a86e-d0193a9078eb)




## Основной сценарий:
1.	Запрос на изменение статуса:
Госслужащий отправляет запрос через CivilServantController для изменения статуса заявки. Запрос включает идентификатор заявки (applicationId) и новый статус (status) через метод UpdateStatus(applicationId, status).
2.	Обработка запроса в сервисе:
CivilServantController передает запрос в CivilServantService, вызвав метод UpdateStatus(applicationId, status).
3.	Поиск заявки в базе данных:
В CivilServantService выполняется запрос в репозиторий ApplicationRepository, чтобы получить заявку по идентификатору через метод FindById(applicationId).
В ApplicationRepository выполняется запрос в базу данных: SELECT * FROM Applications WHERE id = applicationId для получения данных заявки.
4.	Проверка наличия заявки:
Если заявка найдена, данные заявки передаются обратно в CivilServantService.
Если заявка не найдена, система возвращает ошибку в CivilServantController, уведомляя госслужащего о том, что заявка не существует.
5.	Валидация нового статуса:
В CivilServantService выполняется валидация нового статуса заявки. Если статус является допустимым, процесс продолжается. В случае недопустимого статуса, система возвращает ошибку о недопустимом статусе.
6.	Обновление статуса заявки:
Если статус допустим, в CivilServantService вызывает метод UpdateStatus у ApplicationRepository, который выполняет запрос к бд на обновление статуса заявки в базе данных: UPDATE Applications SET status = status WHERE id = applicationId.
После успешного обновления, система возвращает сообщение о том, что статус был успешно изменен.
7.	Ответ госслужащему:
CivilServantController получает результат из CivilServantService и отправляет уведомление госслужащему о том, что статус заявки был успешно обновлен.
8.	Ошибка при недопустимом статусе или отсутствии заявки:
Если статус заявки недопустим или заявка не найдена, возвращаются соответствующие ошибки, и госслужащий получает уведомление об ошибке.

# Актор: Пользователь.

## 8.	Вход пользователя
![image](https://github.com/user-attachments/assets/a3970535-cab4-44d1-a756-6d817be972f6)





## Основной сценарий:
1.	Пользователь отправляет логин и пароль в AuthenticationController.
2.	AuthenticationController передаёт данные в AuthenticationService.
3.	AuthenticationService передает данные на поиск учетной записи в UserRepository.
4.	AuthenticationService запрашивает пользователя по логину через UserRepository:
	UserRepository выполняет запрос к базе данных для поиска пользователя.
	Если пользователь найден:
	Проверяется правильность введённого пароля.
	При совпадении пароля — авторизация успешна, возвращается true.
5.	AuthenticationController сообщает пользователю об успешной авторизации.
## Альтернативные сценарии:
•	Пользователь не найден:

1. После запроса в базу данных UserRepository сообщает, что пользователь отсутствует.

2. AuthenticationService возвращает ошибку "пользователь не найден".

3. AuthenticationController уведомляет пользователя об ошибке.

•	Неверный пароль:

1. При несоответствии пароля AuthenticationService возвращает ошибку неправильного пароля.

2. AuthenticationController сообщает пользователю о неверном пароле.

## 9.	Заявка на регистрацию аккаунта
![image](https://github.com/user-attachments/assets/19eab26a-0f63-4b8f-8f6e-828c1a2bf0d9)






## Основной сценарий:
1.	Пользователь вызывает RegisterUser(login, password, fullName) в AuthenticationController.
2.	AuthenticationController передает данные через RegisterUser(login, password, fullName) в AuthenticationService.
3.	AuthenticationService выполняет:
FindByLogin(login) - проверка существования пользователя
SQL: SELECT * FROM User WHERE login = login.
Получает return null (пользователь не найден).
4.	AuthenticationService создает пользователя:
save(user) - сохранение нового пользователя
SQL: INSERT INTO User (login, password, fullName) VALUES (login, password, fullName).
5.	AuthenticationService → AuthenticationController: return user.
AuthenticationController → Пользователь: "Вы успешно подали заявку на регистрацию".

## Альтернативный сценарий:
1.	При проверке FindByLogin(login):
SQL-запрос находит существующую запись.
Возвращается существующий пользователь.
2.	AuthenticationService прерывает процесс:
Возвращает ошибку: "Пользователь с данным логином уже зарегистрирован".
3.	AuthenticationController показывает сообщение об ошибке пользователю.

## 10.	Подать заявление на оказание услуги
![image](https://github.com/user-attachments/assets/b96ea208-7ad1-4a41-a283-e16b8dc19435)








## Основной сценарий:
1.	Пользователь вызывает метод SubmitApplication(serviceid, userid) в UserController.
2.	UserController передает данные в UserService.
3.	UserService вызывает FindById(userId) в репозитории, который делает запрос к БД: SELECT * FROM User WHERE id = userId.
4.	БД возвращает данные пользователя.
5.	UserService получает даные услуги через ServiceRepository, который делает запрос к БД: SELECT * FROM services WHERE id = serviceId.
6. 	Если услуга найдена, то создается объект заявки, который ApplicationRepository сохраняет в БД.
7. 	Пользователь получает сообщение о том, что заявка создана.

## Альтернативный сценарий:
1.	Пользователь получает сообщение: "Услуга не найдена"

## 11.	Отмена заявления на услугу
![image](https://github.com/user-attachments/assets/4a7b059f-716b-49a0-b79c-a546b4fb5f9f)






## Основной сценарий:
1.	Пользователь инициирует запрос на отмену заявки с помощью метода CancelApplication(applicationId) в UserController.
2.	UserController вызывает метод CancelApplication(applicationId) в UserService.
3.	В UserService происходит запрос в ApplicationRepository для поиска заявки по ID (findbyId(applicationId)).
4.	ApplicationRepository делает SQL-запрос в базу данных для получения данных о заявке:
SELECT * FROM Applications WHERE id = applicationId
5.	Если заявка найдена:
В UserService обновляется статус заявки на "CANCELED" и этот статус сохраняется в базе данных с помощью запроса UPDATE Applications SET status = 'CANCELED' WHERE id = applicationId.
6.	Если заявка не найдена, UserService отправляет ошибку в UserController.
7.	UserController возвращает пользователю сообщение о том, что заявка отменена, либо что заявка не найдена.

## 12.	Узнать статус заявления
![image](https://github.com/user-attachments/assets/e546ae97-4ea0-496d-8277-681778a77af4)





## Основной сценарий:
1.	User получает статус заявки, передавая ApplicationController идентификатор заявки.
2.	ApplicationController вызывает GetApplicationStatus из ApplicationService, который в свою очередь вызывает findbyId из ApplicationRepository.
3.	БД возвращает найденную заявку.
4.	ApplicationService возвращает её статус.
5.	ApplicationController передает статус заявки пользователю.
## Альтернативный сценарий:
Заявка не существует :

1. БД возвращает пустое значение.

2. ApplicationService возвращает ошибку.

3. ApplicationController возвращает сообщение об ошибке.
