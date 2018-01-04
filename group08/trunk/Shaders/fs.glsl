#version 330 core
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct PointLight {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
  
//uniform Material material;
//uniform Light light; 
#define NR_POINT_LIGHTS 4 
uniform PointLight lights[NR_POINT_LIGHTS];
Material material;

uniform vec3 viewPos;
uniform sampler2D s_texture;

in vec3 v_Normal;
in vec3 v_FragPos;
in vec2 v_TexCoord;

out vec4 FragColor;
  
// Function prototypes
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir); 

void main() {
material.ambient = vec3(1.0,0.5,0.31);
material.diffuse = vec3(1.0,0.5,0.31);
material.specular = vec3(0.5,0.5,0.5);
material.shininess = 32.0;

    vec3 result = vec3(0.0, 0.0, 0.0); 
	vec3 norm = normalize(v_Normal);
	vec3 viewDir = normalize(viewPos - v_FragPos);
	
	for(int i = 0; i < NR_POINT_LIGHTS; i++)
		result += CalcPointLight(lights[i], norm, v_FragPos, viewDir);
   
   FragColor = vec4(result, 1.0);
	//FragColor = texture2D(s_texture, v_TexCoord);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	// AMBIENT
	vec3 ambient  = light.ambient * material.ambient;

	// DIFFUSE
	vec3 lightDir = normalize(light.position - v_FragPos);

	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse  = light.diffuse * (diff * material.diffuse);

	//SPECULAR 
	vec3 reflectDir = reflect(-lightDir, normal);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * (spec * material.specular);   

	return ambient + diffuse + specular;
}