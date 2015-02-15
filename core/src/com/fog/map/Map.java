package com.fog.map;

import com.badlogic.gdx.math.Vector2;

public class Map {
	
	private static final int MAP_HEIGHT = 20;
	private static final int MAP_WIDTH = 10;
	
	private int[][] map;
	
	public Map(){
		map = new int[MAP_HEIGHT][MAP_WIDTH];
		map[1][8] = 1; // base 1
		map[18][1] = 2; // base 2
		
		// forest 1
		map[1][1] = 3;
		map[1][2] = 3;
		map[1][3] = 3;
		map[2][1] = 3;
		map[2][2] = 3;
		map[2][3] = 3;
		
		// forest 2
		map[18][8] = 3;
		map[18][7] = 3;
		map[18][6] = 3;
		map[17][8] = 3;
		map[17][7] = 3;
		map[17][6] = 3;
	}
	
	public int getMapValue(Vector2 position) {
		return map[(int) position.y][(int) position.x];
	}
	
	public int getMapValue (int y, int x) {
		return map[y][x];
	}
	
	public int[][] getMap() {
		return map;
	}
}
