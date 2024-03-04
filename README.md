# Manual de Usuario
## Descarga e instalacion
### Pre-requisitos
- Visual Studio 2022
- SQL Server Managment Studio 19(SSMS)
- SQL Server 2022(No importa si es la version Developer o Express)
- ASP.NET Core
- Git

## Descarga e instalacion
- En la carpeta que deses abre una terminal para ingresar el siguiente codigo que clonara todo el proyecto en dicha carpeta:
`git clone https://github.com/CarlosJoaoIpiales/bd-orient-objects.git`
- Ahora en entra en la carpeta donde se descargo el proyecto y busca el archivo llamado `Proyecto_Final.sln` con Visual Studio 2022

## Configuracion de la base de datos de SQL Server
- Abre SQL Server Managment Studio 2019
- Conectate a la base de datos que desees de SQL Server como se ve en la imagen:

![](https://i.ibb.co/SJ50Q55/Captura-de-pantalla-2024-03-04-082211.png)

>Nota: Se debe habilitar o crear un super usuario para que el proyecto pueda hacer cambios en la base de datos

- Una vez dentro, da click derecho en en `Databases` y luego click izquierdo en `New Database` donde debes crear una nueva base de datos con el nombre `ExamenFinal` como se ve en la imagen:

![](https://i.ibb.co/z4H90gH/Captura-de-pantalla-2024-03-04-082627.png)

>Nota: El nombre de la base de datos a crear puede ser cualquiera, pero se debera cambiar este nombre en la cadena de conexion a la base de datos de SQL Server.


## Configuracion de la cadena de conexion a la base de datos desde API.NET Core

- Ahora debemos ir al archivo `appsettings.json` en nuestro proyecto y modificar la cadena de conexion que seria la siguiente:

```
"ConnectionStrings": {
  "BarConnection": "Server=localhost;database=ExamenFinal; User ID=sa; Password=su;integrated security=true; TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```
Donde:
* Server=localhost: En este caso la base de datos funciona de manera local, asi que por ello colocamos `localhost`, en caso de que la base de datos se encuentre en otra parte, debemos colocar la ip de la pc donde se encuetra la base de datos.
* database=ExamenFinal: Aqui va el nombre de la base de datos creada en SQL Server, en caso de haber creado la base de datos con otro nombre, se debe cambiar este parametro con el nombre de la base de datos creada.
* User ID=sa: En este caso habilitamos el super usuario denominado `sa` que viene por defecto en la base de datos, en caso de que creases otro usuario, agrega el nombre de ese usuario aqui.
* Password=su: Aqui va la contraseña del usuario que ingresamos previamente, de igual forma, en caso de tener otra contraseña en el usuario que se ingreso, debe cambiarse en esta linea.

Esto seria todo para la modificacion de la cadena de conexion a la base de datos.

## Migracion a la base de datos en SQL Server
- Ahora ve a herramientas en en la parte superior de Visual Studio, busca la pestaña `Administrador de paquetes de NuGet` y le damos click derecho en `Consola del Administrador de paquetes` como se muestra en la imagen:

![](https://i.ibb.co/NWp7HBg/Captura-de-pantalla-2024-03-04-084452.png)

- Abierta la consola debemos ubicarnos en las opciones de `Proyecto determinado` y escoger la opcion de `DataBase` como se muestra en la imagen:

![](https://i.ibb.co/P9Q7Pwp/Captura-de-pantalla-2024-03-04-084826.png)

- Ahora procedemos a ingresar el siguiente comando para generar una nueva migracion:

```
Add-Migration Migracion1
```

- Una vez generada la migracion, procederemos a actualizar la base de datos en SQL Server con el siguiente comando:

```
Update-Database
```

Una vez hecho esto ya estaria estaria generada nuestra base de datos junto con sus respectivas relaciones como se ve en la imagen:

![](https://i.ibb.co/V2BYZC7/Captura-de-pantalla-2024-03-04-085353.png)


## Configuracion de los controladores
- Una vez hecha la migracion debemos ir al archivo `Program.cs` y bucar el siguiente codigo para proceder a comentarlo:

```
//Funcion para migrar bases de datos
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<BDContext>();
    dataContext.Database.Migrate();
}
```

- Ahora vamos a las clases y vamos a comentar todos los atrubutos en cada clase que sea del tipo `public virtual ICollection<NombreDeLaClase>` para no tener problemas al momento de utilizar la API, como podemos ver a continuacion en la clase "Autor":

```
using DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Autor: IPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
		
		// Se comenta este atributo en todas la clases
        //public virtual ICollection<DetalleLibroAutor>? DetalleLibroAutores { get; set; }
    }
}

```

Y asi segun sea necesario en todas las clases que se tenemos como se muestra en la imagen: 

![](https://i.ibb.co/crxcL94/Captura-de-pantalla-2024-03-04-091134.png)

## Pruebas en la API

- Para realizar las pruebas vamos a SQL Server  donde vamos a generar un nuevo query en nuestra base de datos donde ingresaremos los siguientes scripts:

```
INSERT INTO NivelUsuario (Descripcion)
VALUES
('Estudiante de secundaria'),
('Estudiante universitario'),
('Profesor'),
('Investigador'),
('Bibliotecario');


INSERT INTO Usuario (NivelUsuarioId, DNI, Sexo, Direccion, FechaNacimiento, Email, Telefono, Password)
VALUES
(1, '123456789', 'Femenino', 'Calle 123, Ciudad', '1990-01-01', 'usuario1@email.com', 123456789, 'contrasena1'),
(2, '987654321', 'Masculino', 'Calle ABC, Ciudad', '1985-05-15', 'usuario2@email.com', 987654321, 'contrasena2'),
(3, '555555555', 'Femenino', 'Calle XYZ, Ciudad', '1995-08-20', 'usuario3@email.com', 555555555, 'contrasena3'),
(1, '777777777', 'Masculino', 'Calle 456, Ciudad', '1988-11-10', 'usuario4@email.com', 777777777, 'contrasena4'),
(2, '999999999', 'Femenino', 'Calle 789, Ciudad', '1992-03-25', 'usuario5@email.com', 999999999, 'contrasena5');


INSERT INTO CategoriaLibro (Descripcion)
VALUES
('Novela'),
('Ciencia Ficción'),
('Historia'),
('Ciencia'),
('Arte');


INSERT INTO Autor (Nombres, Apellidos)
VALUES
('Juan', 'Pérez'),
('María', 'Gómez'),
('Carlos', 'Rodríguez'),
('Ana', 'López'),
('Luis', 'Martínez');


INSERT INTO Libro (CategoriaId, Titulo, FechaPublicacion, ISBN, NumeroPaginas, Descripcion)
VALUES
(1, 'Cien años de soledad', '1967-05-30', '9780143039433', 432, 'Una obra maestra de la literatura latinoamericana.'),
(2, 'Dune', '1965-06-01', '9780441172719', 694, 'Epopeya de ciencia ficción que ha dejado huella en el género.'),
(3, 'Sapiens: De animales a dioses', '2014-02-10', '9788499924220', 512, 'Una mirada a la historia de la humanidad.'),
(4, 'El origen de las especies', '1859-11-24', '9780140432053', 502, 'Obra fundamental en la teoría de la evolución.'),
(5, 'El principito', '1943-04-06', '9780156012195', 96, 'Un cuento filosófico y poético para todas las edades.');


INSERT INTO DetalleLibroAutor (LibroId, AutorId)
VALUES
(1, 1),
(1, 2),
(2, 3),
(3, 4),
(4, 5);


ALTER TABLE Prestamo
ALTER COLUMN FechaDevolucion datetime2 NULL;

ALTER TABLE Prestamo
ALTER COLUMN HoraDevolucion datetime2 NULL;

INSERT INTO Prestamo (UsuarioId, EstadoPrestamo, FechaPrestamo, HoraPrestamo, FechaDevolucion, HoraDevolucion)
VALUES
(1, 1, '2024-03-01', '10:30:00', '2024-03-10', '15:45:00'),
(2, 1, '2024-03-02', '11:15:00', '2024-03-12', '14:20:00'),
(3, 1, '2024-03-03', '09:45:00', '2024-03-15', '12:30:00'),
(4, 0, '2024-03-04', '08:00:00', NULL, NULL),
(5, 1, '2024-03-05', '14:00:00', '2024-03-20', '17:10:00');

INSERT INTO DetallePrestamoLibro (PrestamoId,LibroId)
VALUES
(1, 1),
(2, 1),
(3, 2),
(4, 3),
(5, 5);


```
- Una vez hecho todo lo anterior podemos proceder a probar la API con los endpoints que se han generado de cada clase como se ve en la imagen:

![](https://i.ibb.co/6NtRgsD/Captura-de-pantalla-2024-03-04-091813.png)

## Validaciones
### CRUD Autor
- Validación de existencia al obtener un autor por ID:

En el método GetAutor(int id), se verifica si el autor con el ID proporcionado existe en la base de datos. Si no existe, devuelve un resultado 404 (NotFound).

- Validación de existencia antes de agregar un nuevo autor:

En el método PostAutor(Autor autor), se verifica si el autor ya existe en la base de datos antes de agregarlo. Si el autor ya existe, se devuelve un resultado BadRequest con un mensaje indicando que el autor ya está en la base de datos.

- Validación al actualizar un autor por ID:

En el método PutAutor(int id, Autor autor), se verifica si el ID proporcionado coincide con el ID del autor que se está intentando actualizar. Si no coinciden, devuelve un resultado BadRequest con un mensaje indicando la discrepancia.

- Manejo de concurrencia al actualizar un autor:

Se utiliza un bloque try-catch para manejar la excepción DbUpdateConcurrencyException. Si ocurre esta excepción, se verifica si el autor aún existe en la base de datos. Si no existe, devuelve un resultado NotFound.

-Validación antes de eliminar un autor por ID:

En el método DeleteAutor(int id), se verifica si el autor con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.
También verifica si el autor está asociado a algún libro antes de eliminarlo. Si está asociado a algún libro, devuelve un resultado BadRequest con un mensaje indicando que no se puede borrar el autor porque está asociado a uno o más libros.

### CRUD CategoriaLibro

- Validación de existencia al obtener una categoría de libro por ID:

En el método `GetCategoriaLibro(int id)`, se verifica si la categoría de libro con el ID proporcionado existe en la base de datos. Si no existe, devuelve un resultado 404 (NotFound).

- Validación de existencia antes de agregar una nueva categoría de libro:

En el método `PostCategoriaLibro(CategoriaLibro categoriaLibro)`, se verifica si la categoría de libro ya existe en la base de datos antes de agregarla. Si la categoría ya existe, se devuelve un resultado BadRequest con un mensaje indicando que la categoría ya está en la base de datos.

- Validación al actualizar una categoría de libro por ID:

En el método `PutCategoriaLibro(int id, CategoriaLibro categoriaLibro)`, se verifica si el ID proporcionado coincide con el ID de la categoría de libro que se está intentando actualizar. Si no coinciden, devuelve un resultado BadRequest con un mensaje indicando la discrepancia.
Además, se verifica si la categoría ya existe en la base de datos (excepto para la misma categoría). Si la categoría ya existe, se devuelve un resultado BadRequest con un mensaje indicando que la categoría ya está en la base de datos.

- Validación antes de eliminar una categoría de libro por ID:

En el método `DeleteCategoriaLibro(int id)`, se verifica si la categoría de libro con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.
También verifica si la categoría está asociada a algún libro antes de eliminarla. Si está asociada a algún libro, devuelve un resultado BadRequest con un mensaje indicando que no se puede borrar la categoría porque está asociada a uno o más libros.

- Validación de existencia antes de agregar una nueva categoría de libro:

En el método `ExisteCategoria(string nombre)`, se verifica si la categoría de libro con el nombre proporcionado ya existe en la base de datos.

### CRUD DetalleLibroAutor

- Validación al agregar un nuevo detalle libro autor:

En el método `PostDetalleLibroAutor(DetalleLibroAutor detalleLibroAutor)`, se verifica si el libro y el autor existen en la base de datos antes de agregar el detalle libro autor. Si el libro o el autor no existen, se devuelve un resultado BadRequest con un mensaje indicando que el libro o el autor no existen en la base de datos.

- Validación al actualizar un detalle libro autor por ID:

En el método `PutDetalleLibroAutor(int id, DetalleLibroAutor detalleLibroAutor)`, se verifica si el ID proporcionado coincide con el ID del detalle libro autor que se está intentando actualizar. Si no coinciden, devuelve un resultado BadRequest con un mensaje indicando la discrepancia.
También verifica si el libro y el autor existen en la base de datos antes de realizar la actualización. Si el libro o el autor no existen, se devuelve un resultado BadRequest con un mensaje indicando que el libro o el autor no existen en la base de datos.

- Validación antes de eliminar un detalle libro autor por ID:

En el método `DeleteDetalleLibroAutor(int id)`, se verifica si el detalle libro autor con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.

### CRUD Libro

- Validación al agregar un nuevo libro:

En el método `PostLibro(Libro libro)`, se verifica si el libro ya existe en la base de datos antes de agregarlo. Si el libro ya existe, se devuelve un resultado BadRequest con un mensaje indicando que el libro ya está en la base de datos.

- Validación al actualizar un libro por ID:

En el método `PutLibro(int id, Libro libro)`, se verifica si el ID proporcionado coincide con el ID del libro que se está intentando actualizar. Si no coinciden, devuelve un resultado BadRequest.
También verifica si el libro ya existe en la base de datos (excepto para el mismo libro). Si el libro ya existe, se devuelve un resultado BadRequest con un mensaje indicando que el libro ya está en la base de datos.

- Validación antes de eliminar un libro por ID:

En el método `DeleteLibro(int id)`, se verifica si el libro con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.
Valida si el libro está alquilado o prestado. Si el libro está alquilado o prestado, devuelve un resultado BadRequest con un mensaje indicando que no se puede borrar el libro porque está alquilado o prestado.

Elimina los detalles de autor asociados al libro antes de eliminar el libro.

- Verificación de existencia de un libro por ID:

En el método `LibroExists(int id)`, verifica si el libro con el ID proporcionado existe en la base de datos.

- Verificación de existencia de un libro por ISBN:

En el método `VerificarISBNLibro(Libro lib)`, verifica si el libro con el mismo ISBN ya existe en la base de datos.

- Validación si un libro está prestado:

En el método `LibroEstaPrestado(int id)`, verifica si el libro está alquilado o prestado en base a los registros en la tabla `DetallePrestamoLibros`.

### CRUD NivelUsuario

- Validación al agregar un nuevo nivel de usuario:

En el método `PostNivelUsuario(NivelUsuario nivelUsuario)`, se verifica si el nivel de usuario ya existe en la base de datos antes de agregarlo. Si el nivel ya existe, se devuelve un resultado BadRequest con un mensaje indicando que el nivel de usuario ya existe.

- Validación al actualizar un nivel de usuario por ID:

En el método `PutNivelUsuario(int id, NivelUsuario nivelUsuario)`, se verifica si el ID proporcionado coincide con el ID del nivel de usuario que se está intentando actualizar. Si no coinciden, devuelve un resultado BadRequest.
También verifica si el nivel de usuario ya existe en la base de datos (excepto para el mismo nivel). Si el nivel ya existe, se devuelve un resultado BadRequest con un mensaje indicando que el nivel de usuario ya existe.

- Validación antes de eliminar un nivel de usuario por ID:

En el método `DeleteNivelUsuario(int id)`, se verifica si el nivel de usuario con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.
Valida si el nivel de usuario está asociado a uno o más usuarios. Si está asociado, devuelve un resultado BadRequest con un mensaje indicando que no se puede eliminar el nivel porque ya se encuentra asociado a uno o más usuarios.

- Verificación de existencia de un nivel de usuario por descripción:

En el método `NivelUsuarioExists(NivelUsuario nivel)`, verifica si el nivel de usuario con la misma descripción ya existe en la base de datos.

- Verificación de existencia de un nivel de usuario por ID:

En el método `ExistNivel(int id)`, verifica si el nivel de usuario con el ID proporcionado existe en la base de datos.

- Validación si un nivel de usuario está asociado a usuarios:

En el método `NivelAsociado(int id)`, verifica si el nivel de usuario está asociado a uno o más usuarios en base a los registros en la tabla `Usuarios`.

### CRUD Prestamo

- Validación al agregar un nuevo préstamo:

En el método `PostPrestamo(Prestamo prestamo)`, se realizan varias validaciones:
Verifica si el usuario especificado existe en la base de datos. Si no existe, devuelve un resultado BadRequest.
Verifica si la fecha y hora de préstamo son obligatorias.
Valida que la fecha de préstamo no sea menor a la fecha actual.
Valida que cuando el préstamo esté activo, no se ingrese la fecha y hora de devolución.
Valida que no se puedan crear préstamos inactivos.

- Validación al actualizar un préstamo por ID:

En el método `PutPrestamo(int id, Prestamo prestamo)`, se realizan las siguientes validaciones:
Verifica si el ID proporcionado coincide con el ID del préstamo.
Valida si el usuario especificado existe en la base de datos.
Verifica si la fecha y hora de préstamo son obligatorias.
Valida que la fecha de devolución no sea menor a la fecha actual.

- Validación antes de eliminar un préstamo por ID:

En el método `DeletePrestamo(int id)`, se verifica si el préstamo con el ID proporcionado existe. Si no existe, devuelve un resultado NotFound.
Valida que no se pueda eliminar un préstamo activo.

- Verificación de existencia de un usuario por ID:

En el método `VerificarUsuario(int id)`, verifica si el usuario con el ID proporcionado existe en la base de datos.

### CRUD Usuario

- Validación al agregar un nuevo usuario:

En el método `PostUsuario(Usuario usuario)`, se realiza la siguiente validación:
Verifica si el DNI ya está registrado en la base de datos. Si ya está registrado, devuelve un resultado BadRequest.

- Validación al actualizar un usuario por ID:

En el método `PutUsuario(int id, Usuario usuario)`, se realizan las siguientes validaciones:
Verifica si el ID proporcionado coincide con el ID del usuario que se está intentando actualizar.
Verifica si el usuario existe en la base de datos.
Verifica si el DNI ya está registrado en la base de datos. Si ya está registrado, devuelve un resultado BadRequest.

### 3. Validación antes de eliminar un usuario por ID:

En el método `DeleteUsuario(int id)`, se realizan las siguientes validaciones:
Verifica si el usuario con el ID proporcionado existe en la base de datos. Si no existe, devuelve un resultado NotFound.
Verifica si el usuario tiene libros prestados. Si tiene libros prestados, devuelve un resultado BadRequest indicando que no se puede borrar el usuario porque tiene libros prestados.
Elimina los registros de préstamos asociados al usuario antes de borrar el usuario.

- Verificación de existencia de un usuario por ID:

En el método `UsuarioExists(int id)`, verifica si el usuario con el ID proporcionado existe en la base de datos.

- Verificación de existencia de un usuario por DNI:

En el método `UsuarioDniExists(string dni)`, verifica si hay algún usuario registrado con el mismo DNI en la base de datos.

- Verificación de si un usuario tiene libros prestados:

En el método `UsuarioTieneLibrosPrestados(int id)`, verifica si el usuario tiene libros prestados activos.

- Eliminación de préstamos asociados a un usuario:

En el método `EliminarPrestamosUsuario(int usuarioId)`, elimina los registros de préstamos asociados a un usuario.
