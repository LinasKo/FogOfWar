package com.fog.map;

import java.util.ArrayList;

import com.badlogic.gdx.math.Vector2;
import com.fog.units.*;

public class Map {
	
	public static final int MAP_HEIGHT = 20;
	public static final int MAP_WIDTH = 10;
	
	private int[][] map;
	public static ArrayList<Unit> unitList;
	
	public Map(){
		
		// Create Map
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
		
		
		// Create Units
		unitList = new ArrayList<Unit>();
		unitList.add(new Soldier(new Vector2(1.0f, 1.0f), 0));
		unitList.add(new Soldier(new Vector2(2.0f, 2.0f), 0));
	}
	
	public int getMapValue(Vector2 position) {
		return map[(int) position.y][(int) position.x];
	}
	
	public int getMapValue (int y, int x) {
		return map[y][x];
	}
	
	public ArrayList<Unit> getUnits() {
		return unitList;
	}
}
