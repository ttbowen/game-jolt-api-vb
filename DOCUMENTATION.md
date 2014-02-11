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
This function is used to fetch trophies within the game. By using this function, you will return all trophies for your game. A string type is returned from this function - the API response. 

2. **fetchTrophy(ByVal value As Object)** -
This is an overloaded function of "fetchTrophy()" which is used to fetch trophies within the game. This function will accept 1 argument. If you assign True to this argument it will return all achieved trophies in the game and if you assign False it will return all unachieved trophies instead. And if you assign the trophy ID - Integer number then that particular trophy will be returned.  A string type is returned from this function - the API response.

3. **add_TrophyAchieve(ByVal trophyID As Integer)** -
This function achieves a trophy for the user playing the game. To use this function you just pass the trophy ID you wish to be achieved into it's only argument "trophyID". This function will return a boolean value, True if the trophy was successfully added and false if there was an error trying to add the trophy, or if it is already achieved by the user.

Highscores
=============
1. **fetchScores(ByVal userInfo As Boolean, ByVal limit As Integer, ByVal tableID As Integer)** -
This function fetches scores from a specified leaderboard. This function takes 3 arguments "userInfo" which takes a boolean value, "limit" whih takes an integer value and "tableID" which also takes an Integer value type. If ou wish to only fetch scores, achieved by GJ users then assign True to the first argument, else this is false. The second argument will define how many scores you wish to fetch, just enter the umber of scores you wih to fetch; 100 is the limit to this. And for the third argument enter the table ID for where you wish to fetch the scores from; 0 will set this to the primary table. A string type is returned from this function - the API response.

2. **addScores(ByVal score As Integer, ByVal sort As Integer, ByVal Guest As Boolean, ByVal extraData As String, ByVal tableID As Integer)** -
This function adds scores to a specified leaderboard achieved by users playing your game. This function takes 5 arguments which are "score", "sort", "Guest", "extraData" and "tableID". The first argument you assign an Integer value to it, which will be the current score for the player. For the second argument you assign the sort number, the numerical value associated with the current score; this argument is an Integer type. The third argument you assign a boolean value to specify whether the user is a GJ user or guest; True for users and False for guests. The fouth argument is just any extra extra data you wish to store as a String, to be accessed by the API, this is not required. And for the final argument you assign the table ID of where you wish to store the score; this argument is an Integer type. A string type is returned from this function - the API response.

3. **fetchTables()**
This function returns a list of tables for a game and the tables information. A string type is returned from this function - the API response.

Data storage
=============
1. **fetchDataStorage(ByVal key As String, ByVal userInfo As Boolean)** -
This function is used  to fetch various data stored by your game, onto the Game Jolt data storing servers. This function takes 2 arguments which are "key" and "userInfo". The first argument takes a string value which is the key of the data item you would like to fetch. And the second argument takes a boolean to specify whether to collect user specific information or not, True for user information, and False for not fetching user info. A string type is returned from this function - the API response.

2. **setDataStorage(ByVal key As String, ByVal userInfo As Boolean, ByVal data As String)** -
This function sets new data in the Game Jolt data store facility for your game. This function takes 3 arguments, "key", "userInfo" and "data". The first argument you assign the key of the data item, you would like to set; this is a String type. The second argument you specify whether you are setting data for specific users, so you assign True, for users and False to set this globally. And for the third argument you assign the data you wish to set, this should be a String type. A string type is returned from this function - the API response.

3. **removeDataStorage(ByVal key As String, ByVal userInfo As Boolean)** -
This function removes data items from the data storage for your game. This function takes 2 arguments which are "key" and "userInfo". For the first argument you assign the data key for the data item, which will be a String type. And for the second argument you specify whether you wish to romove this data for a user or globally, True sets this to the user and False sets this to be removed globally for the game. A string type is returned from this function - the API response.

4. **getKeysDataStorage(ByVal userInfo As Boolean) As Boolean** -
This function gets data keys stored for your game in the data store. This function takes 1 argument, which takes a boolean type, which you assign True to get user data keys and False to get global data keys stored for your game. A boolean will be returned from this function, True if the data keys was successfully retrieved and False if they were not.

Miscellaneous
=============
1. **New(ByVal private_key As String, ByVal gameID As Integer)** -
This is the class constructor that takes 2 arguments. The first argument you assign a string value, whih is the private key for your game. The private key can be found at Game Jolt by going to "Dashboard" -> "Manage Your Games" then by clicking the game you are adding achievements and then "Achievements". Your private key should be on this page, under "Game info". And for the second argument you assign the game ID for you game found above your private key; makee sure this is an Integer value. This will be called first and set the following properties/class variables to their defaults, these are: GJ_private_key, GJ_gameID , GJ_UserName, GJ_Token, GJ_isActive, GJ_isLoggedIn, GJ_achieved.
