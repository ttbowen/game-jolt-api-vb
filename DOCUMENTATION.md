Documentation
=============
This class will allow you to implement trophies, highscores, data storage and game sessions into your games with the 
Gamejolt API with VB.net. This document will help you to use all of the functions contained within this class and give you an insight of their uses. The documentation here will only provide information on the functions/subs/properties you need to call in your game, the rest contained in the class you do not call. The comments will explain the reason for the function/sub but do not modify it unless really needed.

Contents
=============
1. **Game sessions/Users**
  - fetchUser(ByVal users As List(Of String), ByVal user As Integer)
  - authenticateUser()
  - GJ_Logout()
  - pingSession(ByVal status As Boolean)
  - openSession()
  - closeSession
  - setUserType(ByVal user As user_type)
  - getUserType()

2. **Getters/Setters**
  - SetUserName()
  - SetToken()  
  - getUserName()
  - getToken()
  - getGameID()
  - isLoggedIn()

3. **Trophies**
  - fetchTrophy()
  - fetchTrophy(ByVal value As Object)
  - add_TrophyAchieve(ByVal trophyID As Integer)

4. **Highscores**
  - fetchScores(ByVa*l userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer)
  - addScores(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, ByVal extraData As String, ByVal     tableID As Integer)
  - fetchTables()
  - setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)

5. **Data storage**
  - fetchDataStorage(ByVal key As String, ByVal userInfo As Boolean)
  - setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)
  - removeDataStorage(ByVal key As String, ByVal userInfo As Boolean)
  - getKeysDataStorage(ByVal userInfo As Boolean) As Boolean

6. **Miscellaneous**
  - New(ByVal private_key As String, ByVal gameID As Integer)

Game sessions/Users
=============
1. **fetchUser(ByVal users As List(Of String), ByVal user As Integer)** -
This function is used to fetch Gamejolt user data. The first argument takes a string list type, which you use if you want to fetch more than one user. The second argument takes a single user ID, which you use if you want to fetch data about a single user.

2. **authenticateUser()** -
This function authenticates the user with the user's credentials and opens a new game session if successful. This function returns a  boolean value,  true if credentials are valid and false if they are not. A string type is returned from this function - the API response.


3. **GJ_Logout()** -
This sub logs the user out of the current session and resets the user's username and token. Call this at the point where you need to log the user out.

4. **pingSession(ByVal status As Boolean)** -
This function pings an open session to test if the user is still active or not. This function takes 1 argument called "status" a boolean type, assign false to this "status" if the user is idle and true if the user is active.
If you do not call this function, within 120 seconds the session will automatically close, so it is recommended that you call this function every 30 seconds. A string type is returned from this function - the API response.

5. **openSession()** -
This function opens a new session once the user has been successfully authenticated. Note: This function will have been called at least once, if you have already called the "authenticateUser()" function. A string type is returned from this function - the API response.

6. **closeSession()** -
This function closes an open session with the user, which is also called when you call the "GJ_Logout()" subroutine. A string type is returned from this function - the API response. 

7. **setUserType(ByVal user As user_type)** -
This is a setter property used to set the user type, which will be one of the following: "User", "Developer", "Moderator" and "Admin"; the types are numbered 1-4. You can set the argument to one of these types.

8. **getUserType()** -
This is a getter property used to get the user type, which is set using the "userUserType" property.

Getters/Setters
=============
1. **SetUserName()** -
This is a write only property used to set the user's username. This will be used to authenticate the user. 

2. **SetToken()** - 
This is a write only property used to set the user's token. This will be used to authenticate the user, along with the username. 

3. **getUserName()** -
This is a read only property used to get the user's username.

4. **getToken()** -
This is a read only property used to get the user's token.

5. **getGameID()** -
This is a read only property used to get the game ID.

6. **isLoggedIn()** -
This is a read only property that is used to to check whether the user is currently signed in and in a session with their credentials.

Trophies
=============
1. **fetchTrophy()** -

2. **fetchTrophy(ByVal value As Object)** -

3. **add_TrophyAchieve(ByVal trophyID As Integer)** -

Highscores
=============
1. **fetchScores(ByVa*l userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer)**

2. **addScores(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, ByVal extraData As String, ByVal tableID As Integer)**

3. **fetchTables()**

4. **setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)**

Data storage
=============
1. **fetchDataStorage(ByVal key As String, ByVal userInfo As Boolean)** -

2. **setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)** -

3. **removeDataStorage(ByVal key As String, ByVal userInfo As Boolean)** -

4. **getKeysDataStorage(ByVal userInfo As Boolean) As Boolean** -

Miscellaneous
=============
1. **New(ByVal private_key As String, ByVal gameID As Integer)** -
