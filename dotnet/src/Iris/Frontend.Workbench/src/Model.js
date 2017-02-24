
const initLogs = [
  [0, "[14:12:11] Do laboris fugiat cillum excepteur Lorem officia."],
  [1, "[14:12:11] Ullamco voluptate proident veniam adipisicing nisi esse dolore anim eiusmod."],
  [2, "[14:12:11] Aute nostrud consequat nulla commodo non."],
  [3, "[14:12:11] Non ad incididunt pariatur ullamco sit labore cupidatat aliqua ex consectetur ad dolore."],
  [4, "[14:12:11] Duis consectetur deserunt sint minim culpa aliquip."],
  [5, "[14:12:11] Aute excepteur excepteur quis sint officia incididunt aliquip cillum."],
  [6, "[14:12:11] Officia officia ad adipisicing non."],
  [7, "[14:12:11] Sit qui ullamco cillum Lorem sunt minim sit tempor."],
  [8, "[14:12:11] Laborum officia cillum enim ea sint adipisicing laborum nostrud velit Lorem non commodo dolore."],
]

let counter = initLogs.length;

export default class Model {
  constructor(dispatch) {
    this.subscribers = new Map();
    this.state = {
      widgets: new Map(),
      logs: initLogs
    };
  }

  subscribe(key, subscriber) {
    if (!this.subscribers.has(key)) {
      this.subscribers.set(key, new Map());
    }
    let id = counter++;
    this.subscribers.get(key).set(id, subscriber);
    return {
      dispose() {
        this.subscribers.get(key).delete(id);
      }
    }
  }

  __notify(key, value = this.state[key]) {
    if (this.subscribers.has(key)) {
      this.subscribers.get(key).forEach(subscriber => subscriber(value));
    }
  }

  __setState(key, value) {
    this.state[key] = value;
    this.__notify(key, value);
  }

  addWidget(widget) {
    this.state.widgets.set(counter++, widget);
    this.__notify("widgets");
  }

  removeWidget(id) {
    this.state.widgets.delete(id);
    this.__notify("widgets");
  }

  addLog(log) {
    this.state.logs.splice(0, 0, [counter++, log]);
    this.__notify("logs", this.state.logs);
  }

  removeLastLog() {
    this.state.logs.splice(0, 1);
    this.__notify("logs", this.state.logs);
  }
}