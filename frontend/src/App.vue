<template>
  <div class="app-wrapper">
    <div class="top-accent"></div>

    <main class="content">
      <div class="glass-card">
        <header class="app-header">
          <div class="logo-area">
            <div class="logo-icon">MD</div>
            <div>
              <h1 class="text-dark">MinhaDespensa</h1>
              <span class="text-muted">Controle de estoque doméstico</span>
            </div>
          </div>
        </header>

        <section class="action-bar">
          <div class="input-container">
            <label class="label-green">O que comprou?</label>
            <input v-model="itemName" placeholder="Ex: Arroz Integral" class="dark-field" />
          </div>
          <div class="input-container qty">
            <label class="label-green">Qtd</label>
            <input v-model.number="itemQty" type="number" min="1" class="dark-field" />
          </div>
          <button @click="saveItem" class="main-button">Registrar</button>
        </section>

        <section class="inventory-section">
          <div class="section-header">
            <h2 class="text-dark">Itens no Armário</h2>
            <span class="count-tag">{{ filteredItems.length }} encontrados</span>
          </div>

          <div class="search-box">
            <input v-model="searchQuery" placeholder="🔍 Procurar item..." class="search-field" />
          </div>

          <div class="table-wrapper">
            <div v-if="filteredItems.length === 0" class="empty-state">
              <p class="text-dark">Nenhum item encontrado.</p>
            </div>
            <table v-else>
              <thead>
                <tr>
                  <th class="text-dark">Produto</th>
                  <th class="text-right text-dark">Qtd</th>
                  <th class="text-right text-dark">Ação</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in filteredItems" :key="item.id">
                  <td class="product-name text-dark"><strong>{{ item.name }}</strong></td>
                  <td class="product-qty text-right text-dark">
                    <strong>{{ item.quantity }}</strong>
                  </td>
                  <td class="text-right">
                    <button @click="deleteItem(item.id)" class="delete-btn" title="Remover item">
                      🗑️
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'

const itemName = ref('')
const itemQty = ref(1)
const items = ref([])
const searchQuery = ref('')
const API_URL = 'http://localhost:5079/api/items'

const fetchItems = async () => {
  try {
    const response = await fetch(API_URL)
    items.value = await response.json()
  } catch (error) {
    console.error("Erro ao carregar itens:", error)
  }
}

const filteredItems = computed(() => {
  return items.value.filter(item => 
    item.name.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})

const saveItem = async () => {
  if (!itemName.value) return
  const newItem = { name: itemName.value, quantity: itemQty.value }
  try {
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(newItem)
    })
    if (response.ok) {
      itemName.value = ''
      itemQty.value = 1
      await fetchItems()
    }
  } catch (error) {
    alert("Erro ao conectar com o servidor.")
  }
}

const deleteItem = async (id) => {
  if (!confirm("Deseja realmente remover este item?")) return
  try {
    const response = await fetch(`${API_URL}/${id}`, { method: 'DELETE' })
    if (response.ok) {
      items.value = items.value.filter(item => item.id !== id)
    }
  } catch (error) {
    alert("Erro ao eliminar o item.")
  }
}

onMounted(fetchItems)
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&display=swap');

.app-wrapper {
  font-family: 'Plus Jakarta Sans', sans-serif;
  background-color: #f0f2f0;
  min-height: 100vh;
}

.text-dark { color: #1a1a1a !important; }
.text-muted { color: #4a4a4a !important; font-weight: 600; }
.label-green { color: #008a6e; font-weight: 800; font-size: 0.85rem; margin-bottom: 5px; display: block; }

.top-accent { height: 8px; background: #00b894; }

.content { display: flex; justify-content: center; padding: 40px 20px; }

.glass-card {
  background: white;
  width: 100%;
  max-width: 600px;
  border-radius: 20px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  padding: 30px;
  border: 1px solid #d1d1d1;
}

.action-bar {
  display: flex; gap: 12px;
  background: #2d3436; /* Fundo escuro para destacar o branco */
  padding: 20px;
  border-radius: 15px;
  margin: 20px 0;
}

/* CAMPOS COM FUNDO ESCURO E LETRA BRANCA */
.dark-field {
  background: #3d4446;
  border: 2px solid #00b894;
  padding: 10px;
  border-radius: 8px;
  width: 100%;
  color: #ffffff !important; /* LETRA BRANCA AO DIGITAR */
  font-weight: 700;
  font-size: 1rem;
}

/* Cor do exemplo (Placeholder) em branco suave */
.dark-field::placeholder {
  color: #bdc3c7;
  opacity: 1;
}

.search-field {
  background: #ffffff;
  border: 2px solid #2d3436;
  padding: 12px;
  border-radius: 10px;
  width: 100%;
  color: #000;
  font-weight: 700;
}

.main-button {
  background: #00b894;
  color: white;
  border: none;
  padding: 0 20px;
  border-radius: 8px;
  font-weight: 800;
  cursor: pointer;
  height: 46px;
  align-self: flex-end;
}

.delete-btn {
  background: #fff0f0; border: 1px solid #ffcccc;
  cursor: pointer; font-size: 1.2rem; padding: 8px; border-radius: 8px;
}

table { width: 100%; border-collapse: collapse; }
th { border-bottom: 2px solid #2d3436; padding: 10px; }
td { padding: 15px 10px; border-bottom: 1px solid #e0e0e0; }

.text-right { text-align: right; }
.count-tag {
  background: #00b894; color: white;
  padding: 4px 12px; border-radius: 20px; font-size: 0.8rem; font-weight: 700;
}
</style>