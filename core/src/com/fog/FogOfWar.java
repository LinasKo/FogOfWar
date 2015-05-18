package com.fog;

import com.badlogic.gdx.Game;
import com.fog.screens.GameScreen;
import com.fog.screens.Splash;

public class FogOfWar extends Game {

	// Holds the path to current map
	public static final String PATH_TO_MAP = "maps/TestMap1.tmx";
	
	@Override
	public void create() {
		// setScreen(new Splash(this)); // TODO: uncomment after testing.

		// Remove the following line if not testing. GameScreen is automatically
		// launched from the Splash class.
		setScreen(new GameScreen(FogOfWar.PATH_TO_MAP, 1, 2, 2, 3));
	}
}
