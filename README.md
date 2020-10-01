# Weather
Client weather forecast app written in C# and C++

This application allows you to see the forecast of the weather, according to your current location anywhere in the world

Example of usage:

  1. When you will launch this application you will see this loading screen:
  
  ![alt tag](https://github.com/SergiyLichenko/Weather/blob/master/Docs/Loading%20Screen.png)

  2. After loading the main window ,it looks like this:
  
   ![alt tag](https://github.com/SergiyLichenko/Weather/blob/master/Docs/Main%20Window.png)
   
   You can see the falling snow on the right side of the screenshot - this animation is drawn via DirectX Win2D in C++ and placed on top of the C# form
   
  3. You can also change this animation by clicking button "Switch", in this case you will see your window full of falling snow
  
  ![alt tag](https://github.com/SergiyLichenko/Weather/blob/master/Docs/Another%20snow%20view.png)
  
  4. So as you can see at the center of the screen there is a listbox with days of the week, by clicking on one of the items you will see the weather forecast according to the selected date and to the selected location. The weather forecast is taken from https://openweathermap.org/api
  
  ![alt tag]( https://github.com/SergiyLichenko/Weather/blob/master/Docs/Monday%205.png)
  
  5. You can change you location by typing in the search box at the top. Helper pop-up will appear with variants which are matched according to the text in the textbox. This is done by using Google map API
  
  ![alt tag]( https://github.com/SergiyLichenko/Weather/blob/master/Docs/Montan%20View%20Search.png)
  
  6. After clicking "Search" information will be updated 
  
   ![alt tag](https://github.com/SergiyLichenko/Weather/blob/master/Docs/After%20Select.png)
   
  7. You can also see the location on the Google map by pressing arrow which is located on the left side of the screen
  
  ![alt tag](https://github.com/SergiyLichenko/Weather/blob/master/Docs/Map.png)
  
 
  
