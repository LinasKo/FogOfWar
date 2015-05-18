package com.fog.desktop;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;
import com.fog.FogOfWar;

public class DesktopLauncher {
	public static void main (String[] arg) {
		// Desktop app initialization
		LwjglApplicationConfiguration config = new LwjglApplicationConfiguration();
		new LwjglApplication(new FogOfWar(), config);
	}
}
