<template>
  <div style="padding:16px;">
    <h2>Clients</h2>
    <button @click="load" :disabled="loading">Reload</button>
    <pre v-if="error" style="color:red">{{ error }}</pre>
    <ul v-if="clients.length">
      <li v-for="c in clients" :key="c.id">
        {{ c.firstName }} {{ c.lastName }} — {{ c.email }}
        <span v-if="c.phoneNumbers && c.phoneNumbers.length">
          ({{ c.phoneNumbers.map(p => p.number).join(', ') }})
        </span>
      </li>
    </ul>
    <p v-else-if="!loading">No clients yet.</p>
  </div>
</template>

<script>
import { getClients } from "@/api/clients";
export default {
  name: "Dashboard",
  data: () => ({ clients: [], loading: false, error: "" }),
  created() { this.load(); },
  methods: {
    async load() {
      this.loading = true; this.error = "";
      try { this.clients = (await getClients()).data; }
      catch (e) { this.error = e?.response?.data || e.message; }
      finally { this.loading = false; }
    }
  }
};
</script>
