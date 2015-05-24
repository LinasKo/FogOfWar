package com.fog.structures;

import com.badlogic.gdx.maps.tiled.TiledMapTile;

public class WoodResource extends Resource{
	
    // Constructor
	public WoodResource(TiledMapTile tile, int x, int y, int amount) {
		super(tile, x, y, amount);
	}
	
	
	public void chop(){
		setAmount(getAmount()-1);
	}
}
