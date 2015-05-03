package com.fog;

import com.badlogic.gdx.Game;
import com.fog.screens.Splash;

public class FogOfWar extends Game {

	public static final String MAP_LOCATION = "maps/TestMap1.tmx";
	
	@Override
	public void create() {
		setScreen(new Splash(this));
		// Setting of game screen can be found in the Splash class.
	}
}
