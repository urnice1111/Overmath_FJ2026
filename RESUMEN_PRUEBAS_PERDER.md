# RESUMEN: Pruebas Unitarias para "Perder" en Overmath

## ✅ Lo que he hecho:

### 1. **Adaptación del Procedimiento del Documento**
He tomado la estructura del documento guía sobre pruebas en Snake y la adapté al caso de uso "Perder" de tu proyecto Overmath.

**Diferencias principales:**
- **Snake**: Pruebas sobre movimiento y comer comida
- **Overmath**: Pruebas sobre ganar/perder por puntos y tiempo

### 2. **Investigación del Código**
Analicé los siguientes scripts para entender la lógica de "perder":
- `GameFlowManager.cs` - Controla la detección de ganar/perder
- `TiempoJuego.cs` - Gestiona el tiempo restante (cuenta regresiva)
- `PuntajedePregunta.cs` - Gestiona los puntos acumulados
- `LosePopupUI.cs` - Anima el popup de derrota

### 3. **Lógica de "Perder" Identificada**
En `GameFlowManager.Update()`:
```
Si (tiempoRestante <= 0) Y (puntosActuales < 100):
    → Mostrar derrota
    → Pausar juego (Time.timeScale = 0)
    → Activar LoseCanvas
```

### 4. **Pruebas Creadas**

#### Prueba 1: `TiempoAgota_ConPuntosInsuficientes_MuestraDerrota()`
Verifica que el juego **SÍ muestra derrota** cuando:
- ✅ El tiempo llega a 0
- ✅ Los puntos son < 100

Verificaciones:
- LoseCanvas se activa
- Time.timeScale se pausa (= 0)

#### Prueba 2: `TiempoAgota_ConPuntosSuficientes_MuestraVictoria()`
Verifica que el juego **NO muestra derrota** sino victoria cuando:
- ✅ El tiempo llega a 0
- ✅ Los puntos son >= 100

Verificaciones:
- WinCanvas se activa (victoria)
- LoseCanvas NO se activa
- Time.timeScale se pausa

---

## 📝 Archivos Modificados/Creados:

### Modificado:
- **`Assets/Tests/PlayMode/PerderTest.cs`** ← Las pruebas completas

### Creado:
- **`Assets/Tests/PlayMode/GUÍA_PRUEBAS_PERDER.md`** ← Guía detallada

---

## 🚀 Cómo Ejecutar las Pruebas en Unity:

### Paso 1: Abre Unity
Asegúrate de que no hay errores en la consola antes de empezar.

### Paso 2: Abre el Test Runner
```
Menú superior → Window → General → Test Runner
```

### Paso 3: Selecciona PlayMode
En la ventana del Test Runner, haz clic en la pestaña **PlayMode** (lado derecho).

### Paso 4: Busca las Pruebas
Desplázate y busca **"PerderTest"**. Deberías ver:
```
✓ PerderTest
  ├─ TiempoAgota_ConPuntosInsuficientes_MuestraDerrota
  └─ TiempoAgota_ConPuntosSuficientes_MuestraVictoria
```

### Paso 5: Ejecuta las Pruebas
**Opción A**: Haz clic en **"Run All"** (esquina inferior del Test Runner)
**Opción B**: Haz clic en el ▶️ de PerderTest para ejecutar solo esas pruebas

### Paso 6: Interpreta los Resultados
- **✅ Verde** = Prueba pasó ¡Éxito!
- **❌ Rojo** = Prueba falló (verás un mensaje de error)

---

## ⚠️ Posibles Problemas y Soluciones:

### Problema 1: "No se encuentran las pruebas"
**Solución**:
- Haz clic en "Run All" una vez para forzar el refresco
- Verifica que el archivo esté en `Assets/Tests/PlayMode/`

### Problema 2: "Falla: NullReferenceException"
**Significado**: Falta algún GameObject o componente
**Solución**: 
- Revisa que `PuntajedePregunta`, `TiempoJuego` tienen Singletons (`Instance`)
- En la consola verás exactamente dónde falla

### Problema 3: "Falla: Assertion failed"
**Significado**: La lógica de GameFlowManager no se ejecutó como se esperaba
**Posibles causas**:
- `GameFlowManager.Update()` no se llamó
- Los valores de puntos/tiempo no cambiaron correctamente

---

## 🔍 Cómo Entender el Código de las Pruebas:

Cada prueba sigue el patrón **Arrange-Act-Assert (AAA)**:

```csharp
// ARRANGE - Preparar el escenario
puntajedePregunta.ReiniciarPuntaje();  // Puntos = 0
tiempoJuego.AjustarTiempo(-1000f);     // Tiempo = 0

// ACT - Ejecutar lo que queremos probar
yield return null;  // Un frame de Unity

// ASSERT - Verificar resultados
Assert.IsTrue(loseCanvasObject.activeSelf, "mensaje");
Assert.AreEqual(0f, Time.timeScale, "mensaje");
```

**Cada `Assert` que falla mostrará el mensaje en la consola!**

---

## 💡 Adaptación de lo Aprendido del Documento Guía:

### Del Documento Snake:
✅ Estructura con UnitySetUp/UnityTearDown
✅ Uso de reflexión para acceder campos privados
✅ Patrón AAA (Arrange-Act-Assert)
✅ Comentarios claros en cada sección

### Especiales para Overmath:
✅ Creación de GameObjects en lugar de cargar escenas
✅ Manejo de Singletons (`Instance`)
✅ Pruebas sobre puntos y tiempo (no movimiento)
✅ Pruebas de UI (activar/desactivar Canvas)

---

## 📚 Referencias en tu Proyecto:

Los scripts que necesitarás conocer:

| Script | Ubicación | Responsabilidad |
|--------|-----------|-----------------|
| GameFlowManager | `Sessions&Managers/` | Detecta ganar/perder |
| TiempoJuego | `ScriptsUI/` | Maneja tiempo |
| PuntajedePregunta | `ScriptsUI/` | Maneja puntos |
| LosePopupUI | `AnimationUI/` | Anima popup |

---

## 🎯 Próximas Mejoras (Opcional):

Si quieres agregar más pruebas:

1. **Prueba: Responder incorrectamente reduce tiempo**
   ```csharp
   puntajedePregunta.RegistrarResultado(false);
   // Verifica que tiempo disminuyó en 10
   ```

2. **Prueba: Responder correctamente aumenta puntos**
   ```csharp
   puntajedePregunta.RegistrarResultado(true);
   // Verifica que puntos aumentaron en 5
   ```

3. **Prueba: La animación del popup se ejecuta**
   ```csharp
   // Verifica que LosePopupUI.Show() anima correctamente
   ```

---

## 📌 Respuestas de Reflexión (para tu equipo):

### P1: ¿Qué fue más difícil?
- **ARRANGE**: Entender qué campos privados de GameFlowManager necesitamos
- **ACT**: Saber que "nada pasa sin `yield return null;`"
- **ASSERT**: Pensar en todos los estados que deben cambian

### P2: ¿Qué aprendiste sobre el juego?
- El flujo se controla completamente en GameFlowManager.Update()
- PuntajedePregunta y TiempoJuego son Singletons
- Los canvas se pausa con Time.timeScale (afecta todo el juego)

### P3: ¿Qué otra funcionalidad probarías?
- Los cambios de puntos/tiempo al responder correctamente/incorrectamente
- Que el juego se pausa solo cuando termina (no antes)
- Que los datos persisten al reiniciar

---

## ✉️ Nota Importante:

El documento guía que te dieron es excelente, pero estaba orientado a un juego diferente (Snake).

**Lo que tu caso específico necesitaba:**
- ❌ NO hay movimiento de objetos para probar
- ❌ NO hay comida que comer
- ✅ SÍ hay sistema de puntos y tiempo
- ✅ SÍ hay UI (Canvas) para mostrar/ocultar
- ✅ SÍ hay Singletons

Por eso adapté todo el procedimiento a tu contexto. 🎯

---

**¡Las pruebas están listas para usar! Ejecutalas en el Test Runner y deberían pasar.** ✅

