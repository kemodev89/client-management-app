<template>
  <div style="padding:16px;max-width:700px;margin:auto;">
    <h2>{{ isEdit ? "Edit Client" : "New Client" }}</h2>

    <form @submit.prevent="save" style="display:grid;gap:10px;max-width:600px;">
      <label>
        First name
        <input v-model.trim="model.firstName" required />
      </label>

      <label>
        Last name
        <input v-model.trim="model.lastName" required />
      </label>

      <label>
        Email
        <input v-model.trim="model.email" type="email" required />
      </label>

      <section>
        <h3>Phone Numbers</h3>
        <div v-for="(p,i) in model.phoneNumbers" :key="p.key || p.id || i" style="display:flex;gap:8px;margin:6px 0;">
          <input v-model="p.label" placeholder="Label (e.g. Mobile)" required />
          <input v-model="p.number" placeholder="Number" required />
          <button type="button" @click="removePhone(i)" style="color:#b00020">Remove</button>
        </div>
        <button type="button" @click="addPhone">+ Add phone</button>
      </section>

      <div style="margin-top:8px;">
        <button type="submit">{{ isEdit ? "Save" : "Create" }}</button>
        <button type="button" @click="$router.push({name:'home'})" style="margin-left:6px;">Cancel</button>
      </div>

      <pre v-if="error" style="color:#b00020">{{ error }}</pre>
    </form>
  </div>
</template>

<script>
import { apiGet, apiPost, apiPut } from "../api";

export default {
  name: "ClientForm",
  data: function() {
    return {
      model: { firstName: "", lastName: "", email: "", phoneNumbers: [] },
      loading: false,
      error: ""
    };
  },
  computed: {
    isEdit: function() { return !!this.$route.params.id; },
    id: function() { return Number(this.$route.params.id); }
  },
  created: async function() {
    if (this.isEdit) {
      try { this.model = await apiGet("/clients/" + this.id); }
      catch (e) { this.error = e.message || String(e); }
    } else {
      this.addPhone();
    }
  },
  methods: {
    addPhone: function() {
      this.model.phoneNumbers.push({ id: 0, label: "", number: "", key: Math.random().toString(36).slice(2) });
    },
    removePhone: function(idx) {
      this.model.phoneNumbers.splice(idx, 1);
    },
    save: async function() {
      this.error = ""; this.loading = true;
      try {
        if (this.isEdit) await apiPut("/clients/" + this.id, this.model);
        else await apiPost("/clients", this.model);
        this.$router.push({ name: "home" });
      } catch (e) {
        this.error = e.message || String(e);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>
