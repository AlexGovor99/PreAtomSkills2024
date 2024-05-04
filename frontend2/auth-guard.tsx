"use client";

import { useRouter } from "next/navigation";
import { useCallback, useEffect, useLayoutEffect } from "react";
import { useUser } from "./user";

export function AuthGuard({ children }: any) {
  const router = useRouter();
  const user = useUser();

  useEffect(() => {
    if (user && ["/", "/login"].includes(location.pathname)) router.push("/reports");
    if (!user && !["/login"].includes(location.pathname)) router.push("/login");
  }, [router, user]);

  return <>{children}</>;
}
