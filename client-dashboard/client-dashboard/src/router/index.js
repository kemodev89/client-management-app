import Vue from "vue";
import Router from "vue-router";
import Home from "@/views/Home.vue";
import ClientForm from "@/views/ClientForm.vue";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    { path: "/", name: "home", component: Home },
    { path: "/clients/new", name: "create", component: ClientForm },
    { path: "/clients/:id", name: "edit", component: ClientForm, props: true },
  ],
});
