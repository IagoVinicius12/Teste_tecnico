# Teste_Tecnico

Esse reposit�rio foi feito para o desenvolvimento de uma aplica��o que fizesse o controle de motos, loca��es e entregadores,
de forma que fosse poss�vel fazer confer�ncias e altera��es nos 3 j� citados. 
Na Api foram criados alguns endpoints que necessitam do token de autentca��o para serem feitas as requisi��es, por isso, � necess�rio criar o
usu�rio administrador, para ter acesso a todos os endpoints, e usu�rioe entregador, que tem acesso apenas a alguns endpoints.


# Requisitos para rodar o projeto

O projeto foi desenvolvido usando C# e .NET9, mas foi pensado para rodar em qualquer sistema operacional, por isso � necess�rio ter o docker instalado na m�quina.


# Guia de Uso

Para rodar o projeto, basta executar o comando abaixo na raiz do projeto:
No windows: docker compose up --build -d
No linux: docker-compose up --build -d

# Refer�ncia dos Endpoints

Abaixo est�o os endpoints dispon�veis na API:
motos/create - Cria uma nova moto, necess�rio passar o token de autentica��o no header da requisi��o.
motos/list - Lista todas as motos cadastradas, n�o � necess�rio passar o token de autentica��o no header da requisi��o.
motos/update/{plate} - Atualiza uma moto, necess�rio passar o token de autentica��o no header da requisi��o.
motos/delete/{id} - Deleta uma moto, necess�rio passar o token de autentica��o no header da requisi��o.

locacoes/create - Cria uma nova loca��o, necess�rio passar o token de autentica��o no header da requisi��o.
locacoes/get_locacao/{id} - Busca uma loca��o pelo id, n�o � necess�rio passar o token de autentica��o no header da requisi��o.
locacoes/{id}/update - Atualiza uma loca��o, necess�rio passar o token de autentica��o no header da requisi��o.

# Uso

Quando a aplica��o estiver rodando voc� pode acessar a documenta��o da API atrav�s do Swagger, que estar� disponivil no "Host_de_sua_preferencia:5050/swagger"
Entrando na documenta��o, voc� poder� ver todos os endpoints dispon�veis e testar as requisi��es diretamente pelo navegador.
Lembre-se de que alguns endpoints necessitam do token de autentica��o, que pode ser obtido atrav�s do endpoint de login.
Os tokens de autentica��o devem ser passados no header da requisi��o, por esse motivo foi incluido no Swagger o campo para inserir o token de autentica��o.


