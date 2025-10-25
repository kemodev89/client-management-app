<template>
  <div style="padding:16px;max-width:900px;margin:auto;">
    <header style="display:flex;justify-content:space-between;align-items:center;">
      <h2>Clients</h2>
      <button @click="$router.push({ name: 'create' })">+ New Client</button>
    </header>

    <div style="margin:8px 0;">
      <button @click="load" :disabled="loading">Reload</button>
      <span v-if="loading" style="margin-left:8px;">Loading…</span>
      <pre v-if="error" style="color:#b00020;white-space:pre-wrap">{{ error }}</pre>
    </div>

    <table v-if="clients.length" border="1" cellpadding="6" cellspacing="0" width="100%">
      <thead>
        <tr>
          <th align="left">Name</th>
          <th align="left">Email</th>
          <th align="left">Phone Numbers</th>
          <th width="180">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in clients" :key="c.id">
          <td><strong>{{ c.firstName }} {{ c.lastName }}</strong></td>
          <td>{{ c.email }}</td>
          <td>
            <span v-if="c.phoneNumbers && c.phoneNumbers.length">
              {{ formatPhones(c) }}
            </span>
            <span v-else>—</span>
          </td>
          <td>
            <button @click="$router.push({ name:'edit', params:{ id:c.id }})">Edit</button>
            <button style="margin-left:6px;color:#b00020" @click="remove(c)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else-if="!loading">No clients yet.</p>
  </div>
</template>

<script>
import { apiGet, apiDelete } from "../api";

export default {
  name: "Home",
  data: function() {
    return { clients: [], loading: false, error: "" };
  },
  created: function() { this.load(); },
  methods: {
    async load() {
      this.loading = true; this.error = "";
      try { this.clients = await apiGet("/clients"); }
      catch (e) { this.error = e.message || String(e); }
      finally { this.loading = false; }
    },
    async remove(c) {
      if (!confirm("Delete " + c.firstName + " " + c.lastName + "?")) return;
      try {
        await apiDelete("/clients/" + c.id);
        this.clients = this.clients.filter(function(x){ return x.id !== c.id; });
      } catch (e) {
        alert(e.message || e);
      }
    },
    formatPhones(c) {
      return (c.phoneNumbers || []).map(function(p){
        return (p.label ? p.label + ": " : "") + p.number;
      }).join(", ");
    }
  }
};
</script>
