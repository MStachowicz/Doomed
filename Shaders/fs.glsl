#version 330 core
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
  
//uniform Material material;
//uniform Light light; 

Light light;
Material material;

uniform vec3 viewPos;
uniform sampler2D s_texture;

in vec3 v_Normal;
in vec3 v_FragPos;
in vec2 v_TexCoord;

out vec4 FragColor;
  
void main() {
light.position = vec3(-11.0, 0.0, 0.0);
light.ambient = vec3( 0.2, 0.2, 0.2);
light.diffuse = vec3(0.5,0.5,0.5);
light.specular = vec3(1.0,1.0,1.0);

material.ambient = vec3(1.0,0.5,0.31);
material.diffuse = vec3(1.0,0.5,0.31);
material.specular = vec3(0.5,0.5,0.5);
material.shininess = 32.0;

	// AMBIENT
	vec3 ambient  = light.ambient * material.ambient;

	// DIFFUSE
	vec3 norm = normalize(v_Normal);
	vec3 lightDir = normalize(light.position - v_FragPos);

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse  = light.diffuse * (diff * material.diffuse);

	//SPECULAR 
	vec3 viewDir = normalize(viewPos - v_FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * (spec * material.specular);   

	// RESULT
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
    
	//FragColor = vec4(1.0);
	//FragColor = texture2D(s_texture, v_TexCoord);
}