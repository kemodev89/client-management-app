<template>
  <div style="padding:16px;max-width:900px;margin:auto;">
    <h2>Clients</h2>
    <button @click="load" :disabled="loading">Reload</button>
    <span v-if="loading" style="margin-left:8px;">Loading…</span>
    <pre v-if="error" style="color:#b00020">{{ error }}</pre>

    <ul v-if="clients.length" style="margin-top:12px;">
      <li v-for="c in clients" :key="c.id">
        <strong>{{ c.firstName }} {{ c.lastName }}</strong> — {{ c.email }}
        <span v-if="c.phoneNumbers && c.phoneNumbers.length">
          ({{ c.phoneNumbers.map(p => p.number).join(', ') }})
        </span>
      </li>
    </ul>
    <p v-else-if="!loading">No clients yet.</p>
  </div>
</template>

<script>
export default {
  name: "Home",
  data: () => ({ clients: [], loading: false, error: "" }),
  created() { this.load(); },
  methods: {
    async load() {
      this.loading = true; this.error = "";
      try {
        const url = (process.env.VUE_APP_API || "http://localhost:5000") + "/api/clients";
        const res = await fetch(url);
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        this.clients = await res.json();
      } catch (e) {
        this.error = e.message || String(e);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>
