### TEST SOFTWARE
- Project Todo

## Instalar
- Net Core 3.1
- SQL Server
- Docker
- Docker-compose

## Configurar
- editar con appsettings.json con las credenciales de la db
- actualizar la conexion en Startup.cs
- importar la base de dato Newsan_todo ubicada en db 

## Server archivos
- start `docker-compose up -d`
- stop `docker-compose stop`
- delete `docker-compose down -v`

## Ejecutar
- Open [https://localhost:44338/api/tareas](https://localhost:44338/api/tareas) api.

## Test
- Usar postman para el test de las api
- Importar colleccion de test/Todo - Newsan.postman_collection.json