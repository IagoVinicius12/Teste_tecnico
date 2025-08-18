# Teste_Tecnico

Esse repositório foi feito para o desenvolvimento de uma aplicação que fizesse o controle de motos, locações e entregadores,
de forma que fosse possível fazer conferências e alterações nos 3 já citados. 
Na Api foram criados alguns endpoints que necessitam do token de autentcação para serem feitas as requisições, por isso, é necessário criar o
usuário administrador, para ter acesso a todos os endpoints, e usuárioe entregador, que tem acesso apenas a alguns endpoints.


# Requisitos para rodar o projeto

O projeto foi desenvolvido usando C# e .NET9, mas foi pensado para rodar em qualquer sistema operacional, por isso é necessário ter o docker instalado na máquina.


# Guia de Uso

Para rodar o projeto, basta executar o comando abaixo na raiz do projeto:
No windows: docker compose up --build -d
No linux: docker-compose up --build -d

# Referência dos Endpoints

## Endpoints da API

### Módulo de Motos
| Método | Rota                     | Autenticação | Descrição                                      |
|--------|--------------------------|--------------|------------------------------------------------|
| POST   | `/motos/create`          | Admin        | Cadastra uma nova moto na base de dados        |
| GET    | `/motos/get/{id}`        | Público      | Retorna os dados de uma moto por Identificador |
| GET    | `/motos/list`            | Público      | Lista todas as motos cadastradas               |
| PUT    | `/motos/update/{plate}`  | Admin        | Atualiza os dados de uma moto pela placa       |
| DELETE | `/motos/delete/{id}`     | Admin        | Remove uma moto do sistema por Identificação   |
| GET    | `/getmotobyplate/{plate}`| Admin        | Busca uma moto específica pela placa           |

### 📦 Módulo de Locações
| Método | Rota                       | Autenticação       | Descrição                                |
|--------|----------------------------|--------------------|------------------------------------------|
| POST   | `/rentals/create`          | Admin/Entregador   | Cria um novo contrato de locação         |
| GET    | `/rentals/get_locacao/{id}`| Público            | Consulta uma locação por Identificação   |
| PUT    | `/rentals/{id}/update`     | Admin/Entregador   | Atualiza os dados de uma locação         |

### Módulo de Entregadores
| Método | Rota                             | Autenticação | Descrição                                       |
|--------|----------------------------------|--------------|-------------------------------------------------|
| POST   | `/deliveryperson/create`         | -            | Cadastra um novo entregador                     |
| GET    | `/deliveryperson/list`           | -            | Lista todos os entregadores cadastrados         |
| GET    | `/deliveryperson/get/{id}`       | -            | Busca um entregador específico por Identificação|
| POST   | `/deliveryperson/upload_cnh/{id}`| -            | Upload da CNH (aceita Base64 PNG/BMP)           |

### Módulo Administrativo
| Método | Rota                          | Autenticação | Descrição                          |
|--------|-------------------------------|--------------|------------------------------------|
| POST   | `/admin/create_admin`         | -            | Cria um novo usuário administrador |
| GET    | `/admin/get_admin_by_id/{id}` | -            | Busca um administrador por ID      |

### Autenticação
| Método | Rota          | Autenticação | Descrição                      |
|--------|---------------|--------------|--------------------------------|
| POST   | `/auth/login` | -            | Gera token JWT para acesso     |.


# Uso

Quando a aplicação estiver rodando você pode acessar a documentação da API através do Swagger, que estará disponivil no "Host_de_sua_preferencia:5050/swagger"
Entrando na documentação, você poderá ver todos os endpoints disponíveis e testar as requisições diretamente pelo navegador.
Lembre-se de que alguns endpoints necessitam do token de autenticação, que pode ser obtido através do endpoint de login.
Os tokens de autenticação devem ser passados no header da requisição, por esse motivo foi incluido no Swagger o campo para inserir o token de autenticação.

# Execução:
No windows:<br>
docker compose up --build -d<br>
No linux:<br>
docker-compose up --build -d


