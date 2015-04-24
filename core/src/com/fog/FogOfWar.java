package com.fog;

import com.badlogic.gdx.Game;
import com.fog.screens.Splash;

public class FogOfWar extends Game {

	@Override
	public void create() {
		setScreen(new Splash(this));
		// Setting of game screen can be found in the Splash class.
	}
	// TODO maybe call other methods
}
