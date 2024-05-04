import { useCallback, useEffect, useLayoutEffect, useState } from "react";

export const getUser = () => {
  const user = localStorage.getItem("user");
  if (!user) return null;
  return JSON.parse(user);
};

export const logout = () => {
  localStorage.removeItem("user");
};

let proxifiedLS = false;

export const useUser = () => {
  const [user, setUser] = useState(getUser());

  const cb = useCallback(() => {
    const user = getUser();
    setUser(user);
  }, []);

  if (!proxifiedLS) {
    profixyLS();
    proxifiedLS = true;
  }

  useEffect(() => {
    window.addEventListener("customLsEvent", cb);
  }, [cb]);

  return user;
};

const profixyLS = () => {
  Storage.prototype.setItem = new Proxy(Storage.prototype.setItem, {
    apply(target, thisArg, argumentList) {
      const event = new CustomEvent("customLsEvent", {
        detail: {
          key: argumentList[0],
          oldValue: thisArg.getItem(argumentList[0]),
          newValue: argumentList[1],
        },
      });
      const res = Reflect.apply(target, thisArg, argumentList);
      window.dispatchEvent(event);
      return res;
    },
  });

  Storage.prototype.removeItem = new Proxy(Storage.prototype.removeItem, {
    apply(target, thisArg, argumentList) {
      const event = new CustomEvent("customLsEvent", {
        detail: {
          key: argumentList[0],
        },
      });
      const res = Reflect.apply(target, thisArg, argumentList);
      window.dispatchEvent(event);
      return res;
    },
  });

  Storage.prototype.clear = new Proxy(Storage.prototype.clear, {
    apply(target, thisArg, argumentList) {
      const event = new CustomEvent("customLsEvent", {
        detail: {
          key: "__all__",
        },
      });
      const res = Reflect.apply(target, thisArg, argumentList);
      window.dispatchEvent(event);
      return res;
    },
  });
};
