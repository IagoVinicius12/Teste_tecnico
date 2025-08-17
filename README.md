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

Abaixo estão os endpoints disponíveis na API:
motos/create - Cria uma nova moto, necessário passar o token de autenticação no header da requisição.
motos/list - Lista todas as motos cadastradas, não é necessário passar o token de autenticação no header da requisição.
motos/update/{plate} - Atualiza uma moto, necessário passar o token de autenticação no header da requisição.
motos/delete/{id} - Deleta uma moto, necessário passar o token de autenticação no header da requisição.

locacoes/create - Cria uma nova locação, necessário passar o token de autenticação no header da requisição.
locacoes/get_locacao/{id} - Busca uma locação pelo id, não é necessário passar o token de autenticação no header da requisição.
locacoes/{id}/update - Atualiza uma locação, necessário passar o token de autenticação no header da requisição.

# Uso

Quando a aplicação estiver rodando você pode acessar a documentação da API através do Swagger, que estará disponivil no "Host_de_sua_preferencia:5050/swagger"
Entrando na documentação, você poderá ver todos os endpoints disponíveis e testar as requisições diretamente pelo navegador.
Lembre-se de que alguns endpoints necessitam do token de autenticação, que pode ser obtido através do endpoint de login.
Os tokens de autenticação devem ser passados no header da requisição, por esse motivo foi incluido no Swagger o campo para inserir o token de autenticação.


