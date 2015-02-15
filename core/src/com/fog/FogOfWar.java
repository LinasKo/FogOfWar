package com.fog;

import com.fog.screens.GameScreen;

import com.badlogic.gdx.Game;

public class FogOfWar extends Game {

	@Override
	public void create() {
		setScreen(new GameScreen());
	}
}
