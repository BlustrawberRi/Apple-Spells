shader_type canvas_item;


uniform int outline_size : hint_range(1, 10) = 1;
uniform vec4 outline_color :source_color = vec4(1.0); // White outline

void fragment() {
    vec2 tex_size = vec2(1.f / float(textureSize(TEXTURE, 0).x),1.f / float(textureSize(TEXTURE, 0).y));
    vec4 color = texture(TEXTURE, UV);
    
    // If the pixel is transparent, check surrounding pixels
    if (color.a == 0.0) {
        // Check surrounding pixels
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue; // Skip the center pixel
                vec2 offset = vec2(float(x), float(y)) * float(outline_size) * tex_size;
                vec4 neighbor_color = texture(TEXTURE, UV + offset);
                
                // If any neighbor is not transparent, we draw the outline
                if (neighbor_color.a > 0.0) {
                    color = outline_color;
                    break;
                }
            }
        }
    }
    
    // Set the final color
    COLOR = color;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
