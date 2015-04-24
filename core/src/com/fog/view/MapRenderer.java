package com.fog.view;

import java.util.ArrayList;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer.ShapeType;
import com.badlogic.gdx.math.Rectangle;
import com.fog.map.Map;
import com.fog.units.Unit;

public class MapRenderer {

	private Map map;
	private OrthographicCamera cam;

	/** for debug rendering **/
	ShapeRenderer debugRenderer = new ShapeRenderer();

	public MapRenderer(Map map) {
		this.map = map;
		ArrayList<Unit> unitPool = map.getUnits();
		this.cam = new OrthographicCamera(25, 25); // Field of view size
		this.cam.position.set(5, 10, 0); // Camera position x,y,?
		this.cam.update();
	}

	public void render() {
		debugRenderer.setProjectionMatrix(cam.combined);
		debugRenderer.begin(ShapeType.Filled);
		
		// render map blocks
		for (int row = 0; row < map.MAP_HEIGHT; row++) {
			for (int col = 0; col < map.MAP_WIDTH; col++) {
				Rectangle rect = new Rectangle();
				rect.height = 1;
				rect.width = 1;
				float x1 = col + rect.x;
				float y1 = row + rect.y;
				Color c;
				switch(map.getMapValue(row, col)) {
				case 1:
					c = Color.RED;
					break;
				case 2:
					c = Color.BLUE;
					break;
				case 3:
					c = new Color(0,0.5f,0,0);
					break;
				default:
					c = Color.GREEN;
					break;
				}
				debugRenderer.setColor(c);
				debugRenderer.rect(x1, y1, rect.width, rect.height);
			}
		}
		
		// render units
		debugRenderer.end();
		
	}
}
