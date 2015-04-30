#pragma comment(lib,"Ws2_32.lib")
#include <WinSock2.h>
#include <iostream>
#include <WS2tcpip.h>

SOCKET Connect;
SOCKET* Connections;
SOCKET Listen;


int ClientCount = 0;

void SendMessageToClient(int ID)
{
	char* buffer;
	char* byte_theme;

	for (;; Sleep(75))
	{
		buffer = new char[1024];
		byte_theme = new char[10];
		
		memset(buffer, 0, sizeof(buffer));
		if (recv(Connections[ID], buffer, 1024, NULL))
		{
			printf("%s", buffer);
			for (int i = 0; i <= ClientCount; i++)
			{
				send(Connections[i], buffer, strlen(buffer), NULL);
			}
		}

		memset(byte_theme, 0, sizeof(byte_theme));
		if (recv(Connections[ID], byte_theme, 10, NULL))
		{
			printf("%s", byte_theme);
			for (int i = 0; i <= ClientCount; i++)
			{
				send(Connections[i], byte_theme, strlen(byte_theme), NULL);
			}
		}
	}
	free(buffer); free(byte_theme);
}
int main()
{
	setlocale(LC_ALL, "Russian");
	WSAData data;
	WORD version = MAKEWORD(2, 2);
	int res = WSAStartup(version, &data);
	if (res != 0)
	{
		return 0;
	}

	struct addrinfo hints;
	struct addrinfo * result;


	Connections = (SOCKET*)calloc(64, sizeof(SOCKET));

	ZeroMemory(&hints, sizeof(hints));

	hints.ai_family = AF_INET;
	hints.ai_flags = AI_PASSIVE;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;

	getaddrinfo(NULL, "7770", &hints, &result);
	Listen = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	bind(Listen, result->ai_addr, result->ai_addrlen);
	listen(Listen, SOMAXCONN);

	freeaddrinfo(result);

	printf("Server connect... \n");
	char m_connetc[] = "Connected..."; 
	for (;; Sleep(75))
	{
		if (Connect = accept(Listen, NULL, NULL))
		{
			printf("Client connect...");
			Connections[ClientCount] = Connect;
		//	send(Connections[ClientCount], m_connetc, strlen(m_connetc), NULL); 
			ClientCount++;
			CreateThread(NULL, NULL, (PTHREAD_START_ROUTINE)SendMessageToClient, (LPVOID)(ClientCount - 1), NULL, NULL);
		}

	}
	return 1;
}