![image](https://github.com/user-attachments/assets/cf431331-4617-4ec5-9086-a690879809e3)


# Модели :

## Account

idAcc: id аккаунта

fullName: полное имя пользователя

login: адрес электронной почты

password: пароль

role: роль в системе (пользователь, админ или госслужащий)

##  User (наследуется от Account)

parameters: список всех параметров, введённых пользователем

applications: список всех заявок, поданных пользователем

##  Application

idApplication: id заявки

submissionDate: дата подачи заявки

status: текущий статус заявки

result: результат рассмотрения заявки

completionDate: дата завершения обработки заявки

executionDeadline: дата предполагаемого предоставления услуги

service: услуга, к которой относится заявка

user: пользователь, подавший заявку

##  Service

idService: id услуги

title: название услуги

description: подробное описание услуги

startDate: дата начала оказания услуги

endDate: дата окончания оказания услуги

rules: правила предоставления услуги

##  Rule

idRule: id правила

description: описание условия

value: значение, с которым сравнивается параметр

operator: оператор сравнения (например, >, <, =)

term: cрок, в течение которого должна быть оказана услуга при выполнении данного правила

parameterType: тип параметра, к которому относится правило

##  ParameterType

idParamType: id типа параметра

name: название параметра (например, «доход»)

valueType: тип значения (строка, число, дата и т.п.)

## Parameter

idParameter: id параметра

value: значение, введённое пользователем

parameterType: тип параметра, к которому относится это значение

## Role (enum)

USER — обычный пользователь

ADMIN — администратор

CIVIL_SERVANT — государственный служащий

## ApplicationStatus (enum)

PENDING — заявка ожидает рассмотрения

COMPLETED — заявка обработана

REJECTED — заявка отклонена

CANCELLED — услуга предоставлена

## Operator (enum)

EQUAL — равно

NOT_EQUALS - неравно

GREATER_THAN — больше 

LESS_THAN — меньше

BETWEEN - между

## ValueType (enum)
Тип значения параметра.
Значения:

STRING — строка

INTEGER — число

DATE — дата

BOOLEAN — логическое значение

# Сервисы:

## AuthenticationService

Атрибуты:

userRepository — репозиторий для работы с аккаунтами

Методы:

login(email, password): выполняет вход пользователя по email и паролю

register_user(user): регистрирует нового пользователя на основе переданных данных

## UserService

Атрибуты:

userRepository: репозиторий пользователей

applicationRepository:  репозиторий заявок

parameterRepository: репозиторий параметров

serviceRepository: репозиторий услуг

Методы:

edit_parameter(userId, param, newValue): редактирует значение параметра пользователя

submit_application(application): отправляет заявку от имени пользователя

cancel_application(applicationId): отменяет ранее поданную заявку

## ApplicationService

Атрибуты:

applicationRepository:  репозиторий заявок

Методы:

get_status(applicationId): возвращает текущий статус заявки

get_by_user(userId): возвращает список всех заявок, поданных данным пользователем

get_by_id(applicationId): возвращает заявку по её идентификатору

## CivilServantService

Атрибуты:

applicationRepository: репозиторий заявок

Методы:

process_registration_request(applicationId, approved): принимает или отклоняет регистрацию пользователя

update_status(applicationId, status): обновляет статус заявки

add_result(applicationId, result): добавляет результат к заявке

## AdminService

Атрибуты:

serviceRepository: репозиторий услуг

ruleRepository: репозиторий правил

userRepository: репозиторий пользователей

parameterTypeRepository: репозиторий типов параметров

Методы:

create_service(service): создаёт новую услугу

edit_service(serviceId, service): редактирует существующую услугу

create_rule(rule): добавляет новое правило

edit_rule(ruleId, data): редактирует существующее правило

create_staff_account(user): создаёт учётную запись сотрудника (админа или госслужащего)

create_parameter_type(data): добавляет новый тип параметра

delete_rule(ruleId): удаляет правило

delete_service(serviceId): удаляет услугу

deelete_parameter_type(paramTypeid): удаляет тип параметра

get_all_services(): предоставляет список всех услуг

# Репозитории:

## UserRepository

save(user): сохраняет нового или обновлённого пользователя

find_by_id(userId): ищет пользователя по его идентификатору

find_by_email(email): ищет пользователя по email

delete(userId): удаляет пользователя по идентификатору

findParameters(userId): предоставляет список параметров пользоввателя

## ApplicationRepository

save(application): сохраняет заявку

find_by_id(applicationId): находит заявку по идентификатору

find_by_user(userId): возвращает список заявок конкретного пользователя

delete(applicationId): удаляет заявку по идентификатору

## ServiceRepository

save(service): сохраняет новую услугу

find_by_id(serviceId): возвращает услугу по идентификатору

find_all(): возвращает список всех услуг

find_all_active(): возвращает список всех доступных к получению услуг

update(serviceId, data): обновляет данные услуги

delete(serviceId): удаляет услугу по идентификатору

## RuleRepository

save(rule): сохраняет новое правило

find_by_id(ruleId): возвращает правило по идентификатору

findByServiceId(serviceId): возвращает правила для услуги

update(ruleId, data): обновляет данные правила

delete(ruleId): удаляет правило по идентификатору

## ParameterRepository

save(parameter): сохраняет параметр

find_by_id(parameterId): возвращает параметр по идентификатору

delete(parameterId): удаляет параметр по его идентификатору

update(parameterId): обновляет параметр по его идентификатору

## ParameterTypeRepository

save(parameterType): сохраняет тип параметра

find_by_id(parameterTypeId): возвращает тип параметра по идентификатору

find_all(): возвращает список всех типов параметров

delete(parameterTypeId): удаляет тип параметра

# Связи между классами

AuthenticationService – UserRepository (Агрегация): AuthenticationService содержит UserRepository как компонент для входа и регистрации.

UserService – UserRepository (Агрегация): UserService содержит UserRepository для работы с данными пользователя.

UserService – ApplicationRepository (Агрегация): UserService содержит ApplicationRepository для подачи и отмены заявок.

UserService – ParameterRepository (Агрегация): UserService содержит ParameterRepository для изменения параметров пользователя.

UserService – ServiceRepository (Агрегация): UserService содержит ServiceRepository для получения информации об услугах.

ApplicationService – ApplicationRepository (Агрегация): ApplicationService содержит ApplicationRepository для обработки заявок.

CivilServantService – ApplicationRepository (Агрегация): CivilServantService содержит ApplicationRepository для обновления статусов заявок.

AdminService – ServiceRepository (Агрегация): AdminService содержит ServiceRepository для управления услугами.

AdminService – RuleRepository (Агрегация): AdminService содержит RuleRepository для управления правилами.

AdminService – UserRepository (Агрегация): AdminService содержит UserRepository для создания учётных записей.

AdminService – ParameterTypeRepository (Агрегация): AdminService содержит ParameterTypeRepository для управления типами параметров.

User – Parameter (Агрегация): User использует объекты Parameter для хранения введённых пользователем значений.

User – Application (Агрегация): User использует объекты Application для хранения поданных заявок.

Application – Service (Агрегация): Application использует объект Service для указания услуги.

Service – Rule (Агрегация): Service использует объекты Rule для описания условий предоставления услуги.

Parameter – ParameterType (Агрегация): Parameter использует объект ParameterType для определения своего типа.

Rule – ParameterType (Агрегация): Rule использует объект ParameterType для определения, к какому типу параметра относится.

Account – Role (Агрегация): Account использует перечисление Role для указания роли пользователя.

Application – ApplicationStatus (Агрегация): Application использует перечисление ApplicationStatus для фиксации текущего статуса.

Rule – Operator (Агрегация): Rule использует перечисление Operator для задания логического условия.

ParameterType – ValueType (Агрегация): ParameterType использует перечисление ValueType для указания типа значения.

AuthenticationService – Account (Зависимость): AuthenticationService использует объект Account для входа и регистрации пользователей.

UserService – User (Зависимость): UserService использует объект User для редактирования пользовательских данных.

UserService – Application (Зависимость): UserService использует объект Application для подачи и отмены заявок.

UserService – Parameter (Зависимость): UserService использует объект Parameter для изменения значений параметров пользователя.

UserService – Service (Зависимость): UserService использует объект Service при формировании заявки.

ApplicationService – Application (Зависимость): ApplicationService использует объект Application для получения информации о заявках.

CivilServantService – Application (Зависимость): CivilServantService использует объект Application для обработки заявок и изменения их состояния.

AdminService – Service (Зависимость): AdminService использует объект Service для управления услугами.

AdminService – Rule (Зависимость): AdminService использует объект Rule для управления правилами оказания услуг.

AdminService – Account (Зависимость): AdminService использует объект Account для создания аккаунтов сотрудников.

AdminService – ParameterType (Зависимость): AdminService использует объект ParameterType для создания и редактирования типов параметров.

UserRepository – User (Зависимость): UserRepository использует объект User для сохранения, поиска и удаления пользователей.

ApplicationRepository – Application (Зависимость): ApplicationRepository использует объект Application для управления данными заявок.

ServiceRepository – Service (Зависимость): ServiceRepository использует объект Service для создания, поиска и удаления услуг.

RuleRepository – Rule (Зависимость): RuleRepository использует объект Rule для сохранения и редактирования правил.

ParameterRepository – Parameter (Зависимость): ParameterRepository использует объект Parameter для хранения и обновления пользовательских параметров.

ParameterTypeRepository – ParameterType (Зависимость): ParameterTypeRepository использует объект ParameterType для управления типами параметров.

