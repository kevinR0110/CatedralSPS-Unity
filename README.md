# Catedral SPS 3D

Recorrido virtual 3D de la Catedral de San Pedro Sula, Honduras, desarrollado en Unity. Permite explorar la catedral por fuera y por dentro en primera persona, o rodearla con una cámara orbital, junto con la plaza y el Parque Central que la rodean.

Proyecto individual de la asignatura **Computación Gráfica y Visual** (Ingeniería en Computación).

**Autor:** Robert

---

## Capturas

| Exterior | Interior hacia el altar |
|---|---|
| ![Exterior](docs/exterior.png) | ![Interior](docs/interior.png) |

| Vitral con Bloom | Con y sin postprocesado |
|---|---|
| ![Vitral](docs/vitral.png) | ![Efectos](docs/efectos.png) |

---

## Ficha técnica

| Elemento | Detalle |
|---|---|
| Motor | Unity 6 LTS |
| Pipeline de render | Universal Render Pipeline (URP) |
| Lenguajes | C# y shaders con Shader Graph |
| Modelado | Primitivas de Unity y geometría procedural por código (no se importó ningún modelo) |
| Texturas | PBR de Poly Haven y ambientCG (licencia CC0) |
| Audio | Campanas sintetizadas por código en Python, sin muestras de terceros |
| Control de versiones | Git, GitHub y Git LFS |
| Unidades | 1 unidad de Unity = 1 metro |

**Contenido de la escena:** 14 pilares interiores, 24 bancas, 22 vitrales, 12 luces interiores más el sol, 10 faroles, 8 jardineras hexagonales, 15 árboles y 4 edificios de contexto.

---

## Cómo ejecutarlo

### Opción 1: ejecutable (Windows x64)

1. Descarga el archivo `.zip` desde la sección **Releases** de este repositorio.
2. Descomprímelo en cualquier carpeta.
3. Ejecuta el `.exe` incluido.

### Opción 2: abrir el proyecto en Unity

1. Instala **Unity Hub** y **Unity 6 LTS** con el módulo de la plantilla Universal 3D (URP).
2. Instala **Git** y **Git LFS** (necesario para descargar texturas y audio).
3. Clona el repositorio:
   ```bash
   git lfs install
   git clone <URL-DEL-REPOSITORIO>
   ```
4. Abre la carpeta del proyecto desde Unity Hub.
5. Abre la escena `Assets/Scenes/Catedral_Main` y presiona **Play**.

> En el Player Settings, *Active Input Handling* debe estar en **Both**, porque los scripts usan el Input clásico.

---

## Controles

| Tecla / acción | Función |
|---|---|
| W A S D | Moverse |
| Mouse | Mirar |
| Shift | Correr |
| C | Alternar cámara de primera persona / orbital |
| Orbital: clic y rueda | Girar y acercar |
| H | Mostrar u ocultar el panel de ayuda |
| L | Luces interiores ON/OFF |
| K | Sombras del sol ON/OFF |
| P | Postprocesado ON/OFF |
| N | Niebla ON/OFF |
| V | VSync ON/OFF |
| B | Tocar las campanas |
| Esc | Menú de pausa (Continuar / Salir) |

---

## Características principales

- **Recorrido continuo:** la puerta central está abierta, por lo que se entra a la nave sin pantallas de carga. El personaje sube las gradas y colisiona con muros, pilares y bancas.
- **Dos cámaras:** primera persona con Character Controller y cámara orbital con zoom.
- **Geometría procedural:** las cúpulas se generan por código con el script `Hemisferio`, y los hexágonos del kiosco y las jardineras con `PrismaRegular`.
- **Prefabs repetidos:** pilares, bancas, vitrales y faroles se colocan con el script `Repetir`, de modo que modificar un prefab actualiza todas sus copias.
- **Iluminación diferenciada:** exterior soleado con sombras suaves en tiempo real e interior cálido con 12 luces.
- **Postprocesado y niebla:** Tonemapping, Bloom, Color Adjustments y Vignette, con niebla exponencial cuadrática de baja densidad. Se pueden apagar para comparar.
- **Contador de FPS:** muestra FPS, milisegundos por cuadro y el estado de los efectos.
- **Audio 3D (bono):** dos campanas, una en cada torre, con repique automático cada 90 segundos y un filtro pasa-bajos que apaga el sonido al entrar a la nave.

---

## Shaders propios

### `SG_PBR_Mundo`: materiales PBR con proyección triplanar

Shader Graph de tipo Lit que proyecta las texturas de color, normal y rugosidad en espacio mundo. Así el mismo material se ve correcto en una pared de 30 m y en una banca, sin el estiramiento que produce el mapeo UV en cubos escalados. Expone un tinte, la cantidad de repeticiones por metro, la fuerza del relieve y la metalicidad.

### `SG_Vitral`: mosaico de vidrio con plomo, emisión y Fresnel

Shader procedural, sin imágenes: una rejilla de celdas con un color aleatorio fijo por celda (cuatro colores), líneas de plomo calculadas con `Smoothstep`, emisión para simular la luz que atraviesa el vidrio y un reflejo Fresnel en ángulos rasantes. El vidrio emite su color; no calcula transparencia real.

---

## Optimización

Se midió antes y después de cada cambio, en tres puntos de vista, a 1920 x 1080, con VSync apagado. Técnicas aplicadas:

- **Static batching** sobre la catedral y la plaza (excluyendo las mallas generadas por código).
- **SRP Batcher**, verificado en ambos shaders.
- **Sombras:** sin sombras proyectadas en bancas, vitrales ni suelo; menor distancia, menos cascadas y menor resolución del mapa de sombras. Solo el foco del altar proyecta sombras entre las luces interiores.
- **Luces:** límite de 4 luces por objeto.
- **Texturas:** tamaño máximo de 1024 con mipmaps.

No se usaron LOD ni GPU instancing, porque la escena tiene pocos triángulos y el costo está en draw calls, luces y sombras. El requisito de rendimiento es de 30 FPS mínimo (objetivo 60). Las mediciones completas, con el Frame Debugger y el Profiler, están en el informe técnico.

---

## Estructura del proyecto

```
Assets/
├── Audio/        Campanas en WAV mono
├── Materials/    Materiales PBR y de vitral
├── Models/
├── Prefabs/      Pilar, Banca, Vitral, Farol, Arbol, luces, etc.
├── Scenes/       Catedral_Main
├── Scripts/      Scripts de C#
├── Shaders/      SG_PBR_Mundo, SG_Vitral
└── Textures/     Mapas PBR (color, normal, rugosidad)
```

### Scripts

| Script | Función |
|---|---|
| `Repetir` | Coloca copias de un prefab en fila, con desplazamiento y giro |
| `Hemisferio` | Genera una media esfera por código (domos) |
| `PrismaRegular` | Genera un prisma de N lados por código (hexágonos) |
| `JugadorFPS` | Movimiento y mirada en primera persona |
| `CamaraOrbital` | Cámara que orbita la catedral con zoom |
| `ControlCamaras` | Alterna cámaras (C) y muestra la ayuda (H) |
| `ControlEfectos` | Activa o desactiva postprocesado (P) y niebla (N) |
| `ContadorFPS` | Muestra FPS y milisegundos; VSync con V |
| `MenuPausa` | Pausa con Esc: Continuar y Salir |
| `ControlLuces` | Luces interiores (L) y sombras del sol (K) |
| `CampanasTorres` | Audio 3D de campanas, repique (B) y filtro interior |
| `OptimizacionCatedral` | Script de editor: menú *Catedral > Optimizar* (batching y sombras por lotes) |

---

## Medidas y alcance

Las medidas son **estimadas** a partir de fotografías y un boceto de la planta, no son medidas oficiales. El modelo es un *blockout* simplificado: el ábside se aproxima con paredes rectas y las gradas son rectas. Quedaron fuera del alcance los murales, las imágenes religiosas, las capillas laterales detalladas, el tráfico y las personas.

---

## Créditos y licencias

| Recurso | Fuente | Licencia |
|---|---|---|
| Texturas PBR (estuco, madera, concreto, baldosa, roca, metal, tejas, asfalto, césped) | [Poly Haven](https://polyhaven.com/textures) y [ambientCG](https://ambientcg.com/) | CC0 |
| Campanas | Sintetizadas por código (Python y NumPy) | Propio |
| Modelos 3D | Ninguno importado: todo se construyó con primitivas y código | Propio |

Se utilizó Claude (Anthropic) como asistente de aprendizaje y depuración. El detalle de la declaración de uso de IA está en el informe técnico.

---

## Documentación

- Informe técnico (PDF).
- Presentación del proyecto.
- Video demostrativo de 3 a 5 minutos con el recorrido completo, grabado con el build.
- Capturas y evidencias en la carpeta [`docs/`](docs/).
